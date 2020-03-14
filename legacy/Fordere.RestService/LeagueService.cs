using System.Collections.Generic;
using System.Linq;

using Fordere.RestService.Entities;
using Fordere.RestService.Extensions;
using Fordere.RestService.LeagueExecution;
using Fordere.RestService.LeagueExecution.Standings;
using Fordere.RestService.Properties;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.Competition;
using Fordere.ServiceInterface.Messages.League;

using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class LeagueService : BaseService
    {
        public object Get(GetLeagueByCompetitionIdRequest request)
        {
            var leagues = this.Db.Select(Db.From<League>().Where(p => p.CompetitionId == request.CompetitionId)
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

            return Get(new GetLeagueByIdRequest { Id = request.Id });
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(AddLeagueRequest request)
        {
            var league = request.ConvertTo<League>();
            var id = (int)Db.Insert(league, true);
            return Get(new GetLeagueByIdRequest { Id = id });
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(MoveTeamToLeagueRequest request)
        {
            var team = this.Db.SingleById<Team>(request.TeamId);

            var oldLeagueId = team.LeagueId.GetValueOrDefault();

            using (var transaction = this.Db.BeginTransaction())
            {
                this.Db.Delete<Match>(sql => sql.GuestTeamId == team.Id || sql.HomeTeamId == team.Id && sql.LeagueId == team.LeagueId);

                var targetLeague = this.Db.SingleById<League>(request.Id);
                var teamsInTargetLeague = this.Db.Select<Team>(sql => sql.LeagueId == request.Id);

                var newMatches = MatchFactory.CreateMatchesForMovedTeam(team, teamsInTargetLeague, targetLeague);

                foreach (var match in newMatches)
                {
                    match.LeagueId = targetLeague.Id;
                    this.Db.Insert(match, true);
                }

                this.Db.Update<Team>(new { LeagueId = request.Id }, p => p.Id == request.TeamId);
                this.Db.Update<TableEntry>(new { LeagueId = request.Id }, p => p.TeamId == request.TeamId && p.LeagueId == oldLeagueId);

                StandingsCalculator.Calculate(this.Db, oldLeagueId);
                StandingsCalculator.Calculate(this.Db, request.Id);

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
            var team = new List<TeamInscription> { teamInscription }.CreateTeams().Single();
            var teamId = Db.Insert(team, true);

            Post(new MoveTeamToLeagueRequest
            {
                Id = request.LeagueId,
                TeamId = (int)teamId
            });
        }
    }
}