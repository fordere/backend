using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Final;
using forderebackend.ServiceInterface.FinalDay;
using forderebackend.ServiceModel.Messages.Final;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class MatchEvents
    {
        public async Task TableAssigned(Match match, IDbConnection dbConnection)
        {
            var userAuths = GetPlayersInMatch(match, dbConnection);
            dbConnection.LoadReferences(match);
            await new PlayerContacter().SendMatchAssigned(userAuths, match);
        }

        public async Task Recall(Match match, IDbConnection dbConnection)
        {
            var userAuths = GetPlayersInMatch(match, dbConnection);
            dbConnection.LoadReferences(match);
            await new PlayerContacter().SendRecall(userAuths, match);
        }

        private static List<UserAuth> GetPlayersInMatch(Match match, IDbConnection dbConnection)
        {
            var teamIds = new List<int> { match.HomeTeamId, match.GuestTeamId };
            var teams = dbConnection.Select<Team>(x => Sql.In(x.Id, teamIds));

            var playerIds = new List<int>();
            foreach (var team in teams)
            {
                playerIds.Add(team.Player1Id);
                playerIds.Add(team.Player2Id);
            }

            var userAuths = dbConnection.Select<UserAuth>(x => Sql.In(x.Id, playerIds));
            return userAuths;
        }

        public static void MatchResultChanged(Match match, IDbConnection dbConnection)
        {
            var competitionMode = CompetitionModeFactory.GetCompetitionMode(dbConnection, match);
            var upcommingMatches = competitionMode.GenerateMatchAfterMatchResultEntered(match);
            upcommingMatches.ForEach(x => dbConnection.Insert(x));

            // TODO SSH: Ist das hier am richtigen Ort?
            if (dbConnection.Count<MatchView>(x => x.FinalDayCompetitionId == match.FinalDayCompetitionId && x.HomeTeamScore == null) == 0 && competitionMode.ShouldFinishCompetitionWhenNoMoreMatchesOpen())
            {
                var finalDayCompetition = dbConnection.SingleById<FinalDayCompetition>(match.FinalDayCompetitionId);
                finalDayCompetition.State = FinalDayCompetitionState.Finished;
                dbConnection.Save(finalDayCompetition);
            }
            else
            {
                // TODO SSH: Immer auf running updaten ist eigentlich blöd
                var finalDayCompetition = dbConnection.SingleById<FinalDayCompetition>(match.FinalDayCompetitionId);
                finalDayCompetition.State = FinalDayCompetitionState.Running;
                dbConnection.Save(finalDayCompetition);
            }
        }

        public static void MatchReset(Match match, IDbConnection dbConnection)
        {
            // Make sure the competition is running again -> TODO should we do that always?
            var finalDayCompetition = dbConnection.SingleById<FinalDayCompetition>(match.FinalDayCompetitionId);
            finalDayCompetition.State = FinalDayCompetitionState.Running;
            dbConnection.Save(finalDayCompetition);
        }
    }
}