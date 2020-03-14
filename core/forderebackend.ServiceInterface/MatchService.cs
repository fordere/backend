using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Fordere.RestService.Entities;
using Fordere.RestService.Extensions;
using Fordere.RestService.LeagueExecution;
using Fordere.RestService.LeagueExecution.Standings;
using Fordere.RestService.Properties;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.Match;
using Fordere.ServiceInterface.Messages.Season;
using Fordere.ServiceInterface.Messages.User;

using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    
    public class MatchService : BaseService
    {
        #region HTTP Verbs

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public async Task<object> Patch(PatchMatchRequest request)
        {
            var matchToUpdate = Db.LoadSingleById<Match>(request.Id);

            var incommingFields = Patch(matchToUpdate);

            Db.Update(matchToUpdate, p => p.Id == request.Id);

            await HandleMatchEvents(incommingFields, matchToUpdate, Db);

            return Db.SingleById<Match>(request.Id);
        }

        public object Get(GetMatchByIdRequest request)
        {
            return this.Db.SingleById<MatchView>(request.Id);
        }

        public object Get(GetMatchesForTeamRequest request)
        {
            return Db.Select(Db.From<MatchView>().Where(x => x.HomeTeamId == request.TeamId || x.GuestTeamId == request.TeamId));
        }

        public object Get(UpcommingMatchesRequest request)
        {
            var today = DateTime.UtcNow.AddHours(2).Date;
            var currentSeason = (SeasonDto)base.ResolveService<SeasonService>().Get(new GetCurrentSeasonRequest());

            var matches = this.Db.Select(Db.From<MatchView>().Where(p => p.PlayDate >= today && p.ResultDate == null && p.SeasonId == currentSeason.Id));

            return matches;
        }

        public object Get(RecentMatchesRequest request)
        {
            var currentSeason = (SeasonDto)base.ResolveService<SeasonService>().Get(new GetCurrentSeasonRequest());

            var sevenDaysBack = DateTime.UtcNow.AddHours(2).AddDays(-7);
            var today = DateTime.UtcNow.AddHours(2).Date.AddDays(1).AddSeconds(-1);
            var matches = this.Db.Select(Db.From<MatchView>().Where(p => p.PlayDate >= sevenDaysBack && p.PlayDate <= today && p.ResultDate != null && p.SeasonId == currentSeason.Id));

            return matches;
        }

        public object Get(GetMatchesByLeagueRequest request)
        {
            return this.Db.Select(Db.From<MatchView>().Where(p => p.LeagueId == request.Id));
        }

        public object Get(GetMatchesByCompetitionRequest request)
        {
            return this.Db.Select(Db.From<MatchView>().Where(p => p.CompetitionId == request.Id));
        }

        /// <summary>
        /// Returns the open matches for a user 
        /// </summary>
        public object Get(UserOpenMatchesRequest request)
        {
            return
                this.Db.Select(
                    Db.From<MatchView>().Where(
                            p =>
                                p.ResultDate == null
                                && (p.GuestPlayer1Id == request.UserId || p.GuestPlayer2Id == request.UserId || p.HomePlayer1Id == request.UserId || p.HomePlayer2Id == request.UserId)));
        }

        /// <summary>
        /// Returns the matches for a user. Can optionally be filtered with a seasonid.
        /// </summary>
        public object Get(UserMatchesRequest request)
        {
            if (request.SeasonId.HasValue)
            {
                return this.Db.Select(
                    Db.From<MatchView>().Where(
                            p =>
                                p.SeasonId == request.SeasonId.Value
                                && (p.GuestPlayer1Id == request.UserId || p.GuestPlayer2Id == request.UserId || p.HomePlayer1Id == request.UserId || p.HomePlayer2Id == request.UserId)));
            }

            return
                this.Db.Select(
                    Db.From<MatchView>().Where(
                            p => (p.GuestPlayer1Id == request.UserId || p.GuestPlayer2Id == request.UserId || p.HomePlayer1Id == request.UserId || p.HomePlayer2Id == request.UserId)));
        }

        public object Get(MyMatchesTodayRequest request)
        {
            return this.Db.Select(
                Db.From<MatchView>().Where(
                        p =>
                            (p.GuestPlayer1Id == SessionUserId || p.GuestPlayer2Id == SessionUserId || p.HomePlayer1Id == SessionUserId || p.HomePlayer2Id == SessionUserId) && p.PlayDate >= DateTime.UtcNow.Date &&
                            p.PlayDate <= DateTime.UtcNow.AddDays(1).Date && (p.FinalDayCompetitionId == null || (p.FinalDayCompetitionId != null && p.FinalDayTableNumber != null && !p.IsFreeTicket)))
                        .OrderBy(o => o.PlayDate));
        }

        public object Get(UserMatchesCurrentSeasonRequest request)
        {
            var season = (SeasonDto)base.ResolveService<SeasonService>().Get(new GetCurrentSeasonRequest());


            if (season == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, "There is no current season");
            }

            var request2 = new UserMatchesRequest { SeasonId = season.Id, UserId = request.Id };

            return this.Get(request2);
        }

        [Authenticate]
        public object Put(EnterMatchAppointmentRequest request)
        {
            var match = this.Db.LoadSingleById<Match>(request.Id);

            match.Throw404NotFoundIfNull("Match not found");

            this.EnsureUserMayEnterMatchAppointment(match);

            var universalTime = request.PlayDate.Value.ToUniversalTime();
            this.Db.Update<Match>(new { request.TableId, PlayDate = universalTime, RegisterDate = DateTime.UtcNow }, p => p.Id == request.Id);

            var dto = this.Get(new GetMatchByIdRequest { Id = request.Id });

            return dto;
        }

        [Authenticate]
        public object Delete(DeleteMatchAppointmentRequest request)
        {
            var match = Db.LoadSingleById<Match>(request.MatchId);
            match.Throw404NotFoundIfNull("Match not found");
            EnsureUserMayEnterMatchAppointment(match);

            match.TableId = null;
            match.PlayDate = null;
            match.RegisterDate = null;

            this.Db.Update(match, p => p.Id == request.MatchId);

            var dto = this.Get(new GetMatchByIdRequest { Id = request.MatchId });

            return dto;
        }

        [Authenticate]
        public object Get(GetUserMailsForOpenMatchesInLeagueRequest request)
        {
            var userId = SessionUserId;
            var teamOfUser = Db.Single<Team>(x => x.LeagueId == request.LeagueId && (x.Player1Id == userId || x.Player2Id == userId));
            var matches = Db.Select<Match>(sql => sql.LeagueId == request.LeagueId && sql.PlayDate == null && sql.RegisterDate == null && (sql.HomeTeamId == teamOfUser.Id || sql.GuestTeamId == teamOfUser.Id));

            var mailList = new List<string>();
            var teamIds = matches.Select(s => s.HomeTeamId).Concat(matches.Select(s => s.GuestTeamId)).Distinct().ToList();

            var teams = this.Db.SelectByIds<Team>(teamIds).ToDictionary(k => k.Id);
            var userIds = teams.Values.Select(s => s.Player1Id).Concat(teams.Values.Select(s => s.Player2Id)).ToList();
            var users = this.Db.SelectByIds<UserAuth>(userIds).ToDictionary(k => k.Id);

            foreach (var match in matches)
            {
                if (match.HomeTeamId == teamOfUser.Id)
                {
                    var guestTeam = teams[match.GuestTeamId];
                    if (!guestTeam.IsForfaitOut)
                    {
                        mailList.Add(users[guestTeam.Player1Id].Email);
                        mailList.Add(users[guestTeam.Player2Id].Email);
                    }
                }
                else
                {
                    var homeTeam = teams[match.HomeTeamId];
                    if (!homeTeam.IsForfaitOut)
                    {
                        mailList.Add(users[homeTeam.Player1Id].Email);
                        mailList.Add(users[homeTeam.Player2Id].Email);
                    }
                }
            }

            return new UserMailsDto { UserMails = string.Join(",", mailList) };
        }

        [Authenticate]
        public object Delete(DeleteMatchResultRequest request)
        {
            var match = Db.LoadSingleById<Match>(request.MatchId);

            EnsureUserMayEnterMatchResult(match);

            match.HomeTeamScore = null;
            match.GuestTeamScore = null;
            match.ResultDate = null;

            Db.Update(match);

            StandingsCalculator.Calculate(this.Db, match.LeagueId);

            return Get(new GetMatchByIdRequest { Id = request.MatchId });
        }

        [Authenticate]
        public object Put(EnterMatchResultRequest request)
        {
            ValidateResult(request);

            using (var transaction = this.Db.BeginTransaction())
            {
                var match = this.Db.LoadSingleById<Match>(request.Id);

                match.Throw404NotFoundIfNull("Match not found");

                this.EnsureUserMayEnterMatchResult(match);

                if (match.PlayDate == null)
                {
                    this.Db.Update<Match>(new { PlayDate = DateTime.UtcNow }, p => p.Id == request.Id);
                }

                this.Db.Update<Match>(new { request.HomeTeamScore, request.GuestTeamScore, ResultDate = DateTime.UtcNow }, p => p.Id == request.Id);

                var dto = this.Get(new GetMatchByIdRequest { Id = request.Id });

                StandingsCalculator.Calculate(this.Db, match.LeagueId);

                transaction.Commit();

                return dto;
            }
        }

        private void ValidateResult(EnterMatchResultRequest request)
        {
            var match = this.Db.Single<MatchView>(x => x.Id == request.Id);
            if (match.CompetitionId.HasValue)
            {
                if (request.HomeTeamScore > 3 || request.GuestTeamScore > 3)
                {
                    throw new ArgumentException("Mehr als drei Sätze kann ein Team nicht gewinnen!");
                }

                int? numberOfPlayedSets = request.HomeTeamScore + request.GuestTeamScore;
                if (numberOfPlayedSets != 3 && numberOfPlayedSets != 4 && numberOfPlayedSets != 5)
                {
                    throw new ArgumentException("Du hast eine unmögliche Anzahl an gespielter Sätze eingegeben!");
                }

                if (numberOfPlayedSets == 5 && (request.HomeTeamScore > 3 || request.GuestTeamScore > 3))
                {
                    throw new ArgumentException("Wenn ein Entscheidungssatz gespielt wurde kannst du nicht mehr als drei Sätze gewonnen haben");
                }

                if (numberOfPlayedSets == 4 && (request.HomeTeamScore == 4 || request.GuestTeamScore == 4))
                {
                    throw new ArgumentException("Bei einem Best of 5 ist das Spiel bei 3:0 beendet. Trage bitte 3:0 als Resultat ein.");
                }
            }
            else
            {
                if (request.HomeTeamScore != 10 && request.GuestTeamScore != 10)
                {
                    throw new ArgumentException("Mindest ein Team muss 10 Tore geschossen haben");
                }

                if (request.HomeTeamScore == request.GuestTeamScore)
                {
                    throw new ArgumentException("Ein Unentschieden ist im Cup nicht möglich!");
                }

                if (request.HomeTeamScore > 10 || request.GuestTeamScore > 10)
                {
                    throw new ArgumentException("Mehr als 10 Tore ist nicht möglich!");
                }
            }
        }

        #endregion

        public MatchViewDto Put(UpdateMatchRequest request)
        {
            var matchToUpdate = Db.SingleById<Match>(request.Id);
            matchToUpdate.PopulateWith(request);

            if (matchToUpdate.HasResult && !matchToUpdate.ResultDate.HasValue)
            {
                matchToUpdate.ResultDate = DateTime.Now;
            }

            if (matchToUpdate.PlayDate.HasValue && !matchToUpdate.RegisterDate.HasValue)
            {
                matchToUpdate.ResultDate = matchToUpdate.PlayDate;
            }

            matchToUpdate.IsNotPlayedMatch = false;

            Db.Save(matchToUpdate);

            StandingsCalculator.Calculate(this.Db, matchToUpdate.LeagueId);

            return Db.SingleById<MatchView>(request.Id).ConvertTo<MatchViewDto>();
        }

        public MatchViewDto Post(ResetMatchRequest request)
        {
            var matchToReset = Db.SingleById<Match>(request.Id);
            matchToReset.PlayDate = null;
            matchToReset.RegisterDate = null;
            matchToReset.ResultDate = null;
            matchToReset.HomeTeamScore = null;
            matchToReset.GuestTeamScore = null;
            matchToReset.TableId = null;
            matchToReset.IsNotPlayedMatch = false;
            Db.Save(matchToReset);

            StandingsCalculator.Calculate(this.Db, matchToReset.LeagueId);

            return Db.SingleById<MatchView>(request.Id).ConvertTo<MatchViewDto>();
        }

        private async Task HandleMatchEvents(List<string> incommingFields, Match match, IDbConnection dbConnection)
        {
            if (new[] { "PlayDate", "RegisterDate", "FinalDayTableId" }.All(incommingFields.Contains) && incommingFields.Count == 3 && match.FinalDayTableId.HasValue)
            {
                await new MatchEvents().TableAssigned(match, dbConnection);
            }
            else if (incommingFields.Count == 1 && incommingFields.Contains("PlayDate"))
            {
                await new MatchEvents().Recall(match, dbConnection);
            }
            else if (new[] { "ResultDate", "HomeTeamScore", "GuestTeamScore" }.All(incommingFields.Contains) && incommingFields.Count == 3)
            {
                MatchEvents.MatchResultChanged(match, dbConnection);
            }
            else if (new[] { "ResultDate", "HomeTeamScore", "GuestTeamScore", "PlayDate", "FinalDayTableId" }.All(incommingFields.Contains) && incommingFields.Count == 5)
            {
                MatchEvents.MatchReset(match, dbConnection);
            }
        }

        #region Security

        private void EnsureUserMayEnterMatchAppointment(Match match)
        {
            if (this.IsAdmin)
            {
                return;
            }

            if (match.ResultDate.HasValue)
            {
                throw new UnauthorizedAccessException();
            }

            if (match.GuestTeam.Player1Id == this.SessionUserId ||
                match.GuestTeam.Player2Id == this.SessionUserId ||
                match.HomeTeam.Player1Id == this.SessionUserId ||
                match.HomeTeam.Player2Id == this.SessionUserId)
            {
                return;
            }

            throw new UnauthorizedAccessException();
        }

        private void EnsureUserMayEnterMatchResult(Match match)
        {
            if (this.IsAdmin)
            {
                return;
            }

            if (match.ResultDate.HasValue && DateTime.UtcNow.Subtract(match.ResultDate.Value) > TimeSpan.FromMinutes(10))
            {
                throw new UnauthorizedAccessException("10 Minuten sind abgelaufen. Du kannst das Resultat nicht mehr ändern.");
            }

            if (match.GuestTeam.Player1Id == this.SessionUserId ||
                match.GuestTeam.Player2Id == this.SessionUserId ||
                match.HomeTeam.Player1Id == this.SessionUserId ||
                match.HomeTeam.Player2Id == this.SessionUserId)
            {
                return;
            }

            throw new UnauthorizedAccessException();
        }

        #endregion
    }
}