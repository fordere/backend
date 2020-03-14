using System.Collections.Generic;
using System.Data;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Final;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.FinalDay
{
    // TODO SSH: Cleanup & Performance (nr. of selects...)
    public static class CompetitionTeamStandingsCalculator
    {
        public const int PointsWin = 3;
        public const int PointsDraw = 1;

        public static void Calculate(IDbConnection db, int groupId)
        {
            var teams = db.LoadSelect<TeamInGroup>(x => x.GroupId == groupId).Select(x => x.Team).ToList();
            var teamStandings = db.Select<CompetitionTeamStanding>(x => x.GroupId == groupId);
            var matches = GetMatchesInGroup(db, groupId);

            EnsureEachTeamHasTableEntry(db, groupId, teams, teamStandings);

            teamStandings = db.LoadSelect<CompetitionTeamStanding>(x => x.GroupId == groupId);

            UpdateTableEntries(db, teams, matches, teamStandings);

            UpdateRanks(db, teamStandings, groupId);
        }

        private static List<Match> GetMatchesInGroup(IDbConnection db, int groupId)
        {
            var group = db.LoadSingleById<Group>(groupId);
            var teamIds = group.Teams.Select(x => x.TeamId).ToList();
            return db.Select<Match>(x =>
                x.FinalDayCompetitionId == group.FinalDayCompetitionId &&
                (Sql.In(x.HomeTeamId, teamIds) || Sql.In(x.GuestTeamId, teamIds)));
        }

        private static void UpdateRanks(IDbConnection db, IList<CompetitionTeamStanding> tableEntries, int groupId)
        {
            var rank = 1;

            var matches = GetMatchesInGroup(db, groupId);
            var orderedTableEntries = CompetitionTeamStandingsSorter.OrderByRanking(tableEntries, matches);
            foreach (var tableEntry in orderedTableEntries)
            {
                var entry = tableEntry;

                tableEntry.Rank = rank;
                db.Update<CompetitionTeamStanding>(new {Rank = rank}, p => p.Id == entry.Id);
                ++rank;
            }
        }

        private static void UpdateTableEntries(IDbConnection db, IEnumerable<Team> teams, List<Match> matches,
            List<CompetitionTeamStanding> tableEntries)
        {
            foreach (var team in teams)
            {
                var matchesOfTeam = matches.Where(p => p.HomeTeamId == team.Id || p.GuestTeamId == team.Id).ToList();
                var playedMatchesOfTeam = matchesOfTeam.Where(p => p.HasResult).ToList();
                var tableEntry = tableEntries.First(p => p.TeamId == team.Id);

                tableEntry.GamesPlayed = playedMatchesOfTeam.Count;

                tableEntry.GamesWon = playedMatchesOfTeam.Count(p => p.WinnerTeamId == team.Id);

                tableEntry.GamesDraw = playedMatchesOfTeam.Count(p => p.IsDraw);

                tableEntry.GamesLost = playedMatchesOfTeam.Count(p => p.WinnerTeamId != team.Id && !p.IsDraw);

                tableEntry.GoalsScored = playedMatchesOfTeam.Where(p => p.HomeTeamId == team.Id)
                                             .Sum(s => s.HomeTeamScore.GetValueOrDefault())
                                         + playedMatchesOfTeam.Where(p => p.GuestTeamId == team.Id)
                                             .Sum(s => s.GuestTeamScore.GetValueOrDefault());

                tableEntry.GoalsConceded = playedMatchesOfTeam.Where(p => p.HomeTeamId == team.Id)
                                               .Sum(s => s.GuestTeamScore.GetValueOrDefault()) +
                                           playedMatchesOfTeam.Where(p => p.GuestTeamId == team.Id)
                                               .Sum(s => s.HomeTeamScore.GetValueOrDefault());

                var winsWithoutOvertime = playedMatchesOfTeam.Count(p => p.WinnerTeamId == team.Id);
                var draws = playedMatchesOfTeam.Count(p => p.IsDraw);


                tableEntry.Points = winsWithoutOvertime * PointsWin + draws * PointsDraw;

                db.Update(tableEntry, p => p.Id == tableEntry.Id);
            }
        }

        private static void EnsureEachTeamHasTableEntry(IDbConnection db, int groupId, IEnumerable<Team> teams,
            List<CompetitionTeamStanding> standings)
        {
            foreach (var team in teams)
            {
                var competitionTeamStanding = standings.SingleOrDefault(s => s.TeamId == team.Id);

                if (competitionTeamStanding == null)
                {
                    competitionTeamStanding = new CompetitionTeamStanding {GroupId = groupId, TeamId = team.Id};
                    competitionTeamStanding.Id = (int) db.Insert(competitionTeamStanding, true);
                }
            }
        }
    }
}