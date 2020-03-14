using System.Collections.Generic;
using System.Data;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Final;
using forderebackend.ServiceInterface.LeagueExecution;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.FinalDay
{
    public class GroupCompetitionMode : ICompetitionMode
    {
        private readonly IDbConnection dbConnection;

        public GroupCompetitionMode(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        // TODO SSH Duplicated code with SingleKOCompetitionMode
        public List<Match> GenerateMatches(int finalDayCompetitionId)
        {
            var groups = dbConnection.LoadSelect<Group>(x => x.FinalDayCompetitionId == finalDayCompetitionId);
            var generatedMatches = new List<Match>();

            foreach (var finalDayGroup in groups)
            {
                var teams = finalDayGroup.Teams;
                generatedMatches.AddRange(GenerateMatchesForGroup(teams));
            }

            generatedMatches.ForEach(x => x.FinalDayCompetitionId = finalDayCompetitionId);

            return generatedMatches;
        }

        private List<Match> GenerateMatchesForGroup(List<TeamInGroup> teamsInGroup)
        {
            if (teamsInGroup == null)
                // Wenn keine Teams der Gruppe zugewiesen wurden müssen
                // auch keine Matches erstellt werden
                return new List<Match>();

            var teams = dbConnection.SelectByIds<Team>(teamsInGroup.Select(x => x.TeamId));
            return new SingleRoundMatchCreator().CreateMatches(teams);
        }

        public List<Match> GenerateMatchAfterMatchResultEntered(Match match)
        {
            // TODO SSH eigentlich würde es reichen nur die vom match betroffene Gruppe zu aktualisieren
            var groups = dbConnection.Select<Group>(x => x.FinalDayCompetitionId == match.FinalDayCompetitionId.Value);
            foreach (var group in groups) CompetitionTeamStandingsCalculator.Calculate(dbConnection, @group.Id);

            return new List<Match>();
        }

        public void AfterMatchSafe(List<Match> dayCompetitionId, int finalDayCompetitionId)
        {
            var groups = dbConnection.Select<Group>(x => x.FinalDayCompetitionId == finalDayCompetitionId);
            groups.ForEach(g => CompetitionTeamStandingsCalculator.Calculate(dbConnection, g.Id));
        }

        public long GetNumberOfMatches(int finalDayCompetitionId)
        {
            return dbConnection.Count(dbConnection.From<MatchView>()
                .Where(x => x.FinalDayCompetitionId == finalDayCompetitionId));
        }

        public bool ShouldFinishCompetitionWhenNoMoreMatchesOpen()
        {
            return true;
        }
    }
}