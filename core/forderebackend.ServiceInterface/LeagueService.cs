﻿using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Extensions;
using forderebackend.ServiceInterface.LeagueExecution;
using forderebackend.ServiceInterface.LeagueExecution.Standings;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.Competition;
using forderebackend.ServiceModel.Messages.League;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class LeagueService : BaseService
    {
        public object Get(GetLeagueByCompetitionIdRequest request)
        {
            var leagues = Db.Select(Db.From<League>().Where(p => p.CompetitionId == request.CompetitionId)
                .OrderBy(o => o.Number)
                .ThenBy(o => o.Group));

            return leagues.Select(s => s.ConvertTo<LeagueDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetLeagueByIdRequest request)
        {
            return Db.SingleById<League>(request.Id).ConvertTo<LeagueDto>();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Put(UpdateLeagueRequest request)
        {
            var league = Db.SingleById<League>(request.Id);
            league.Throw404NotFoundIfNull("Competition not found");

            league.PopulateWith(request);

            Db.Update(league);

            return Get(new GetLeagueByIdRequest {Id = request.Id});
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(AddLeagueRequest request)
        {
            var league = request.ConvertTo<League>();
            var id = (int) Db.Insert(league, true);
            return Get(new GetLeagueByIdRequest {Id = id});
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(MoveTeamToLeagueRequest request)
        {
            var team = Db.SingleById<Team>(request.TeamId);

            var oldLeagueId = team.LeagueId.GetValueOrDefault();

            using (var transaction = Db.BeginTransaction())
            {
                Db.Delete<Match>(sql =>
                    sql.GuestTeamId == team.Id || sql.HomeTeamId == team.Id && sql.LeagueId == team.LeagueId);

                var targetLeague = Db.SingleById<League>(request.Id);
                var teamsInTargetLeague = Db.Select<Team>(sql => sql.LeagueId == request.Id);

                var newMatches = MatchFactory.CreateMatchesForMovedTeam(team, teamsInTargetLeague, targetLeague);

                foreach (var match in newMatches)
                {
                    match.LeagueId = targetLeague.Id;
                    Db.Insert(match, true);
                }

                Db.Update<Team>(new {LeagueId = request.Id}, p => p.Id == request.TeamId);
                Db.Update<TableEntry>(new {LeagueId = request.Id},
                    p => p.TeamId == request.TeamId && p.LeagueId == oldLeagueId);

                StandingsCalculator.Calculate(Db, oldLeagueId);
                StandingsCalculator.Calculate(Db, request.Id);

                transaction.Commit();
            }

            return null;
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(CreateTeamAndMatchFromTeamInscriptionRequest request)
        {
            // Assign league to teaminscription
            var teamInscription = Db.SingleById<TeamInscription>(request.TeamInscriptionId);
            teamInscription.AssignedLeagueId = request.LeagueId;
            Db.Update(teamInscription);

            // Create team from inscription
            var team = new List<TeamInscription> {teamInscription}.CreateTeams().Single();
            var teamId = Db.Insert(team, true);

            Post(new MoveTeamToLeagueRequest
            {
                Id = request.LeagueId,
                TeamId = (int) teamId
            });
        }
    }
}