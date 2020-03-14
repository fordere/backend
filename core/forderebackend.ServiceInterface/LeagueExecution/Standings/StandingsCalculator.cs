using System.Collections.Generic;
using System.Data;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.LeagueExecution.Standings.OrderRules;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.LeagueExecution.Standings
{
    public static class StandingsCalculator
    {
        public const int SetsWinForfait = 3;
        public const int SetsLossForfait = 0;
        public const int PointsWin = 3;
        public const int PointsDraw = 1;
        public const int PointsWinOvertime = 2;
        public const int PointsLossOvertime = 1;
        public const int SetsWinOvertime = 3;
        public const int SetsLossOvertime = 2;
        public const int PointsHomeTeamNotPlayedMatch = 0;
        public const int PointsGuestTeamNotPlayedMatch = 0;
        public const int MaxNumberOfNotPlayedMatches = 2;
        public const int SetsHomeTeamNotPlayedMatch = 0;
        public const int SetsGuestTeamNotPlayedMatch = 0;

        public static void Calculate(IDbConnection db, int leagueId)
        {
            var teams = db.Select(db.From<Team>().Where(p => p.LeagueId == leagueId));
            var tableEntries = db.Select(db.From<TableEntry>().Where(p => p.LeagueId == leagueId));
            var matches = db.LoadSelect(db.From<Match>().Where(p => p.LeagueId == leagueId));

            EnsureEachTeamHasTableEntry(db, leagueId, teams, tableEntries);

            tableEntries = db.LoadSelect(db.From<TableEntry>().Where(p => p.LeagueId == leagueId));

            UpdateTableEntries(db, teams, matches, tableEntries);

            UpdateRanks(db, tableEntries, OrderRuleFactory.CreateOrderRules(db, leagueId));
        }

        private static void UpdateRanks(IDbConnection db, IEnumerable<TableEntry> tableEntries,
            List<IOrderRule> orderRules)
        {
            var rank = 1;

            var orderedTableEntries = tableEntries.OrderBy(x => x.Team.IsForfaitOut);
            foreach (var orderRule in orderRules)
            {
                orderedTableEntries = orderedTableEntries.ThenByDescending(x => x, orderRule);
            }

            foreach (var tableEntry in orderedTableEntries)
            {
                var entry = tableEntry;

                tableEntry.Rank = rank;
                db.Update<TableEntry>(new {Rank = rank}, p => p.Id == entry.Id);
                ++rank;
            }
        }

        private static void UpdateTableEntries(IDbConnection db, IEnumerable<Team> teams, List<Match> matches,
            List<TableEntry> tableEntries)
        {
            foreach (var team in teams)
            {
                var matchesOfTeam = matches.Where(p => p.HomeTeamId == team.Id || p.GuestTeamId == team.Id).ToList();
                var playedMatchesOfTeam = matchesOfTeam.Where(p => p.HasResult).ToList();
                var notPlayedMatches = matchesOfTeam.Where(x => x.IsNotPlayedMatch).ToList();
                var tableEntry = tableEntries.First(p => p.TeamId == team.Id);

                var homeTeamNotPlayed = matchesOfTeam.Count(x => x.IsNotPlayedMatch && x.HomeTeamId == team.Id);
                var guestTeamNotPlayed = matchesOfTeam.Count(x => x.IsNotPlayedMatch && x.GuestTeamId == team.Id);

                tableEntry.GamesNotPlayed = matchesOfTeam.Count(x => x.IsNotPlayedMatch);
                tableEntry.GamesPlayed = playedMatchesOfTeam.Count;

                tableEntry.GamesWon = playedMatchesOfTeam.Count(p => p.WinnerTeamId == team.Id && !p.IsExtensionPlayed);
                tableEntry.GamesWonExtension =
                    playedMatchesOfTeam.Count(p => p.WinnerTeamId == team.Id && p.IsExtensionPlayed);

                tableEntry.GamesDraw = playedMatchesOfTeam.Count(p => p.IsDraw);

                tableEntry.GamesLostExtension =
                    playedMatchesOfTeam.Count(p => p.WinnerTeamId != team.Id && !p.IsDraw && p.IsExtensionPlayed);
                tableEntry.GamesLost =
                    playedMatchesOfTeam.Count(p => p.WinnerTeamId != team.Id && !p.IsDraw && !p.IsExtensionPlayed);

                tableEntry.SetsWon = playedMatchesOfTeam.Where(p => p.HomeTeamId == team.Id)
                                         .Sum(s => s.HomeTeamScore.GetValueOrDefault())
                                     + playedMatchesOfTeam.Where(p => p.GuestTeamId == team.Id)
                                         .Sum(s => s.GuestTeamScore.GetValueOrDefault()) +
                                     notPlayedMatches.Count(p => p.GuestTeamId == team.Id) *
                                     SetsGuestTeamNotPlayedMatch +
                                     notPlayedMatches.Count(p => p.HomeTeamId == team.Id) * SetsHomeTeamNotPlayedMatch;

                tableEntry.SetsLost = playedMatchesOfTeam.Where(p => p.HomeTeamId == team.Id)
                                          .Sum(s => s.GuestTeamScore.GetValueOrDefault()) +
                                      playedMatchesOfTeam.Where(p => p.GuestTeamId == team.Id)
                                          .Sum(s => s.HomeTeamScore.GetValueOrDefault()) +
                                      notPlayedMatches.Count(p => p.HomeTeamId == team.Id) *
                                      SetsGuestTeamNotPlayedMatch +
                                      notPlayedMatches.Count(p => p.GuestTeamId == team.Id) *
                                      SetsHomeTeamNotPlayedMatch;

                var winsWithoutOvertime =
                    playedMatchesOfTeam.Count(p => p.Overtime == false && p.WinnerTeamId == team.Id);
                var winsInOvertime = playedMatchesOfTeam.Count(p => p.Overtime && p.WinnerTeamId == team.Id);
                var lossInOvertime = playedMatchesOfTeam.Count(p => p.Overtime && p.WinnerTeamId != team.Id);
                var draws = playedMatchesOfTeam.Count(p => p.IsDraw);


                tableEntry.Points = (winsWithoutOvertime * PointsWin) + (winsInOvertime * PointsWinOvertime) +
                                    (draws * PointsDraw) + (lossInOvertime * PointsLossOvertime) +
                                    (homeTeamNotPlayed * PointsHomeTeamNotPlayedMatch) +
                                    (guestTeamNotPlayed * PointsGuestTeamNotPlayedMatch);

                db.Update(tableEntry, p => p.Id == tableEntry.Id);
            }
        }

        private static void EnsureEachTeamHasTableEntry(IDbConnection db, int leagueId, IEnumerable<Team> teams,
            List<TableEntry> tableEntries)
        {
            foreach (var team in teams)
            {
                var tableEntry = tableEntries.SingleOrDefault(s => s.TeamId == team.Id);

                if (tableEntry == null)
                {
                    tableEntry = new TableEntry {LeagueId = leagueId, TeamId = team.Id};
                    tableEntry.Id = (int) db.Insert(tableEntry, true);
                }
            }
        }
    }
}