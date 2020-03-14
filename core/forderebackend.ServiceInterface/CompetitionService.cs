using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Extensions;
using forderebackend.ServiceInterface.LeagueExecution;
using forderebackend.ServiceInterface.LeagueExecution.Standings;
using forderebackend.ServiceInterface.Smtp;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.Competition;
using forderebackend.ServiceModel.Messages.LeagueRegistration;
using forderebackend.ServiceModel.Messages.News;
using forderebackend.ServiceModel.Messages.Season;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class CompetitionService : BaseService
    {
        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Put(UpdateCompetitionRequest request)
        {
            var competition = Db.SingleById<Competition>(request.Id);
            competition.Throw404NotFoundIfNull("Competition not found");

            competition.PopulateWith(request);

            Db.Update(competition);

            return Get(new GetCompetitionByIdRequest {Id = request.Id});
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(AddCompetitionRequest request)
        {
            var competition = request.ConvertTo<Competition>();
            var id = (int) Db.Insert(competition, true);
            return Get(new GetCompetitionByIdRequest {Id = id});
        }

        public object Get(GetCompetitionStateRequest request)
        {
            var competitionId = request.CompetitionId;

            var leagues = Db.LoadSelect<League>(x => x.CompetitionId == competitionId);
            var hasGeneratedTeams = leagues.Where(x => x.Teams != null).SelectMany(x => x.Teams).Any();
            if (hasGeneratedTeams)
            {
                return new CompetitionStateDto {Value = (int) CompetitionState.Running};
            }

            // Any not assigned teaminscriptions?
            var hasNotAssigendTeams =
                Db.Select<TeamInscription>(t => t.CompetitionId == competitionId && t.AssignedLeagueId == null).Any();

            if (hasNotAssigendTeams)
            {
                return new CompetitionStateDto {Value = (int) CompetitionState.NotAssignedTeams};
            }

            var hasGeneratedMatches = Db.Select<MatchView>(x => x.CompetitionId == competitionId).Any();
            if (!hasGeneratedMatches)
            {
                return new CompetitionStateDto {Value = (int) CompetitionState.ReadyForGenerate};
            }

            // TODO: Finished... 
            return new CompetitionStateDto {Value = (int) CompetitionState.Running};
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Get(SetMatchesNotPlayedRequest request)
        {
            var matchIds = Db.Select(Db.From<MatchView>().Where(p =>
                p.CompetitionId == request.CompetitionId && p.PlayDate == null && !p.HomeTeamIsForfaitOut &&
                !p.GuestTeamIsForfaitOut)).Select(x => x.Id);
            Db.Update<Match>(new {IsNotPlayedMatch = true}, x => matchIds.Contains(x.Id));

            var competition = Db.LoadSingleById<Competition>(request.CompetitionId);
            foreach (var league in competition.Leagues) StandingsCalculator.Calculate(Db, league.Id);
        }

        public object Get(GetCompetitionsOpenForRegistration request)
        {
            // Find all seasons which are currently in registration state
            var seasonsWithActiveRegistrationPeriod = Db.Select(Db.From<Season>()
                .Where(x => x.State == SeasonState.Registration || x.State == SeasonState.PrepareSeason)
                .Where(x => x.DivisionId == DivisionId));

            // Find all competitions for the given seasons
            var competitions = Db.Select<Competition>()
                .Where(x => seasonsWithActiveRegistrationPeriod.Any(t => t.Id == x.SeasonId)).ToList();
            var compDtos = competitions.Select(x =>
            {
                var dto = x.ConvertTo<OpenRegistrationCompetitionDto>();
                dto.CurrentUserTeamInscription = FindTeamInscriptionForCurrentUser(x.Id);
                return dto;
            }).ToList();

            return compDtos;
        }

        private TeamInscriptionDto FindTeamInscriptionForCurrentUser(int competitionId)
        {
            var teamInscription = Db.Single<TeamInscription>(x =>
                (x.Player1Id == SessionUserId || x.Player2Id == SessionUserId) && x.CompetitionId == competitionId);

            if (teamInscription == null)
            {
                return null;
            }

            teamInscription.Player1 = Db.SingleById<UserAuth>(teamInscription.Player1Id);
            teamInscription.Player2 = Db.SingleById<UserAuth>(teamInscription.Player2Id);

            return teamInscription.ConvertTo<TeamInscriptionDto>();
        }

        public object Get(GetAllCompetitionsRequest request)
        {
            var competitions = Db.LoadSelect<Competition>();
            return competitions.Select(s => s.ConvertTo<CompetitionDto>());
        }

        public object Get(GetAllCompetitionsInSeasonRequest request)
        {
            var competitions = Db.LoadSelect<Competition>(sql => sql.SeasonId == request.SeasonId);
            return competitions.Select(s => s.ConvertTo<CompetitionDto>());
        }

        public object Get(GetAllCompetitionsInCurrentRequest request)
        {
            var currentSeason = (SeasonDto) base.ResolveService<SeasonService>().Get(new GetCurrentSeasonRequest());
            return Get(new GetAllCompetitionsInSeasonRequest {SeasonId = currentSeason.Id});
        }

        public object Get(GetCompetitionByIdRequest request)
        {
            var competition = Db.LoadSingleById<Competition>(request.Id);

            return competition;
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(CreateTeamsAndMatchesRequest request)
        {
            using (var transaction = Db.OpenTransaction(IsolationLevel.ReadCommitted))
            {
                long competitionId = request.CompetitionId;

                var teamInscriptions = Db.Select(Db.From<TeamInscription>()
                    .Where(p => p.CompetitionId == competitionId && p.AssignedLeagueId != null));
                var teams = teamInscriptions.CreateTeams();

                teams.ForEach(team => team.Id = (int) Db.Insert(team, true));

                var matches = MatchFactory
                    .CreateLeagueMatches(teams, leagueId => Db.SingleById<League>(leagueId).LeagueMatchCreationMode)
                    .ToList();

                Db.InsertAll(matches);

                var leagueIds = matches.Select(s => s.LeagueId).Distinct().ToList();
                leagueIds.ForEach(leagueId => StandingsCalculator.Calculate(Db, leagueId));

                transaction.Commit();
            }
        }

        public object Get(GetStandingsFromSeasonRequest request)
        {
            var competitions = Db.Select<Competition>(sql => sql.SeasonId == request.SeasonId);
            var competitionsDtos = new List<CompetitionDto>();

            competitions.ForEach(x =>
                competitionsDtos.Add((CompetitionDto) Get(new GetStandingsFromCompetitionsRequest {Id = x.Id})));

            return competitionsDtos;
        }

        public object Get(GetStandingsFromCompetitionsRequest request)
        {
            var competition = Db.LoadSingleById<Competition>(request.Id);
            var competitionDto = competition.ConvertTo<CompetitionDto>();
            competitionDto.Leagues = new List<LeagueDto>();

            competition.Throw404NotFoundIfNull("competition not found");

            foreach (var league in competition.Leagues.OrderBy(x => x.Number))
            {
                //TableCalculator.Calculate(Db, league.Id);

                var leagueDto = league.ConvertTo<LeagueDto>();
                leagueDto.TableEntries = Db.Select(Db.From<TableEntryView>().Where(p => p.LeagueId == league.Id))
                    .Select(s => s.ConvertTo<TableEntryViewDto>()).OrderBy(o => o.Rank).ToList();
                competitionDto.Leagues.Add(leagueDto);
            }

            return competitionDto;
        }

        public object Get(GetTeamsFromCompetitionsRequest request)
        {
            var competition = Db.LoadSingleById<Competition>(request.Id);

            competition.Throw404NotFoundIfNull("competition not found");

            var leagueIds = competition.Leagues.Select(s => s.Id).ToList();

            if (!leagueIds.Any())
            {
                return null;
            }

            var allTeams = Db.Select(Db.From<TeamView>().Where(p => Sql.In(p.LeagueId, leagueIds)));

            var dto = new List<LeagueWithTeamsDto>();

            foreach (var league in competition.Leagues.OrderBy(x => x.Number))
            {
                var leagueDto = league.ConvertTo<LeagueWithTeamsDto>();
                leagueDto.Teams = allTeams.Where(p => p.LeagueId == league.Id).Select(s => s.ConvertTo<TeamViewDto>())
                    .ToList();

                dto.Add(leagueDto);
            }

            return dto;
        }

        [Authenticate]
        public object Post(CompetitionRegisterRequest request)
        {
            request.Player1Id = SessionUserId;

            var competition = Db.LoadSingleById<Competition>(request.CompetitionId);

            if (competition == null)
            {
                throw HttpError.NotFound("League Registration not found");
            }

            if (request.Player1Id == request.Player2Id)
            {
                throw new ArgumentException("Sorry you're skill level is too low");
            }

            if (!CheckPlayersNotAlreadyRegistered(request.Player1Id, request.Player2Id, request.CompetitionId))
            {
                throw new ArgumentException("One of the players is already registered");
            }

            if (request.Player1Id != SessionUserId && request.Player2Id != SessionUserId)
            {
                throw new ArgumentException("You try to register a team you are not a member of...");
            }

            var entity = request.ConvertTo<TeamInscription>();

            var newId = Db.Insert(entity, true);

            var player1 = Db.Single<UserAuth>(x => x.Id == SessionUserId);
            var player2 = Db.Single<UserAuth>(x => x.Id == entity.Player2Id);
            var bar = Db.Single<Bar>(x => x.Id == entity.BarId);

            CreatePaymentEntry(player1, competition.SeasonId);
            CreatePaymentEntry(player2, competition.SeasonId);

            MailSender.SendRegistrationConfirmation(DivisionId, entity.Name, entity.SeasonAmbition, entity.WishPlayDay,
                player1, player2, competition, bar);

            return new HttpResult(null, HttpStatusCode.Created)
            {
                Location = Request.AbsoluteUri + "/teaminscriptions/" + newId
            };
        }

        private void CreatePaymentEntry(UserAuth user, int seasonId)
        {
            if (!Db.Select<Payment>().Any(x => x.UserId == user.Id && x.SeasonId == seasonId))
            {
                Db.Insert(new Payment {UserId = user.Id, SeasonId = seasonId});
            }
        }

        private bool CheckPlayersNotAlreadyRegistered(int player1, int player2, int competitionId)
        {
            var count = Db.Count<TeamInscription>(p => p.CompetitionId == competitionId &&
                                                       (p.Player1Id == player1 ||
                                                        p.Player2Id == player1 ||
                                                        p.Player1Id == player2 ||
                                                        p.Player2Id == player2));

            return count == 0;
        }
    }
}