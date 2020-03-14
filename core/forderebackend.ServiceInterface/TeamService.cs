using System.Data;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Extensions;
using forderebackend.ServiceInterface.LeagueExecution;
using forderebackend.ServiceInterface.LeagueExecution.Standings;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.Team;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    
    public class TeamService : BaseService
    {
        public object Get(GetTeamByIdRequest request)
        {
            var team = Db.SingleById<Team>(request.Id);

            team.Throw404NotFoundIfNull("Team not found");

            team.Player1 = Db.LoadSingleById<UserAuth>(team.Player1Id);
            team.Player2 = Db.LoadSingleById<UserAuth>(team.Player2Id);

            var teamDto = team.ToDto();

            return teamDto;
        }

        private bool IsCurrentUserPlayingInLeague(int leagueId)
        {
            long currentUserId = SessionUserId;
            return Db.Select<Team>(sql => (sql.Player1Id == currentUserId || sql.Player2Id == currentUserId) && sql.LeagueId == leagueId).Any();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(UpdateTeamRequest request)
        {
            var originalTeam = this.Db.SingleById<Team>(request.Id);
            bool shouldUpdateStandings = originalTeam.IsForfaitOut != request.IsForfaitOut;
            originalTeam.PopulateWith(request);
            Db.Save(originalTeam);

            if (shouldUpdateStandings && originalTeam.LeagueId.HasValue)
            {
                StandingsCalculator.Calculate(this.Db, originalTeam.LeagueId.Value);
            }

            return Db.SingleById<Team>(request.Id).ConvertTo<TeamDto>();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Patch(UpdateTeamRequest request)
        {
            var originalTeam = this.Db.SingleById<Team>(request.Id);
            originalTeam.PopulateWithNonDefaultValues(request);
            Db.Save(originalTeam);
            return Db.SingleById<Team>(request.Id).ConvertTo<TeamDto>();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Delete(DeleteTeamRequest request)
        {
            var team = this.Db.SingleById<Team>(request.Id);
            var leagueId = team.LeagueId.GetValueOrDefault();

            team.Throw404NotFoundIfNull("Team existiert nicht!");

            using (var transaction = this.Db.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                this.Db.Delete<TableEntry>(sql => sql.TeamId == team.Id);

                if (this.Db.Count<Match>(p => (p.HomeTeamId == team.Id || p.GuestTeamId == team.Id) && p.CupId != 0) > 0)
                {
                    // team already has cup matches -> we do not delete cup matches and therefore we 
                    // have to keep the team, but remove league assignment and delete the league matches
                    team.LeagueId = null;
                    this.Db.Update(team);
                    this.Db.Delete<Match>(p => (p.HomeTeamId == team.Id || p.GuestTeamId == team.Id) && p.LeagueId > 0);

                    MatchFactory.ForfaitOpenCupMatchesForTeam(team.Id, this.Db);
                }
                else
                {
                    this.Db.Delete<Match>(sql => sql.GuestTeamId == team.Id || sql.HomeTeamId == team.Id);
                    this.Db.Delete<Team>(sql => sql.Id == team.Id);
                }

                
                if (leagueId != default(int))
                {
                    StandingsCalculator.Calculate(this.Db, leagueId);
                }

                transaction.Commit();
            }

            return null;
        }
    }
}