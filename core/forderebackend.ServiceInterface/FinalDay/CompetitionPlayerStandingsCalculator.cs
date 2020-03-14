using System.Collections.Generic;
using System.Data;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Final;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.FinalDay
{
    public static class CompetitionPlayerStandingsCalculator
    {
        // TODO SSH Make it configurable
        public const int PointsWin = 3;
        public const int PointsDraw = 1;

        public static void Calculate(IDbConnection db, int finalDayCompetitionId)
        {
            var players = db.LoadSelect<PlayerInFinalDayCompetition>(x => x.FinalDayCompetitionId == finalDayCompetitionId).Select(x => x.Player).ToList();
            var standings = db.Select<CompetitionPlayerStanding>(x => x.FinalDayCompetitionId == finalDayCompetitionId);
            var matches = db.Select<MatchView>(x => x.FinalDayCompetitionId == finalDayCompetitionId);

            EnsureEachPlayerHasTableEntry(db, players, standings, finalDayCompetitionId);

            standings = db.Select<CompetitionPlayerStanding>(x => x.FinalDayCompetitionId == finalDayCompetitionId);

            UpdateStandings(db, players, matches, standings);

            UpdateRanks(db, standings);
        }

        private static void UpdateRanks(IDbConnection db, List<CompetitionPlayerStanding> standings)
        {
            var rank = 1;

            // TODO SSH Add Buchholz comparer
            var orderedTableEntries = standings.OrderByDescending(p => p.Points).ThenByDescending(o => o.PlusMinus).ThenByDescending(p => p.GamesWon);
            foreach (var tableEntry in orderedTableEntries)
            {
                var entry = tableEntry;

                tableEntry.Rank = rank;
                db.Update<CompetitionPlayerStanding>(new { Rank = rank }, p => p.Id == entry.Id);
                ++rank;
            }
        }

        private static void UpdateStandings(IDbConnection db, List<UserAuth> players, List<MatchView> matches, List<CompetitionPlayerStanding> standings)
        {
            foreach (var player in players)
            {
                var matchesOfTeam = matches.Where(p => p.HomePlayer1Id == player.Id || p.HomePlayer2Id== player.Id || p.GuestPlayer1Id == player.Id || p.GuestPlayer2Id == player.Id).ToList();
                var playedMatchesOfTeam = matchesOfTeam.Where(p => p.HasResult).ToList();
                var tableEntry = standings.First(p => p.PlayerId == player.Id);

                tableEntry.GamesPlayed = playedMatchesOfTeam.Count;

                tableEntry.GamesWon = playedMatchesOfTeam.Count(p => p.WinnerTeamPlayer1 == player.Id || p.WinnerTeamPlayer2 == player.Id);

                tableEntry.GamesDraw = playedMatchesOfTeam.Count(p => p.IsDraw);

                tableEntry.GamesLost = playedMatchesOfTeam.Count(p => p.WinnerTeamPlayer1 != player.Id && p.WinnerTeamPlayer2 != player.Id && !p.IsDraw);

                tableEntry.GoalsScored = playedMatchesOfTeam.Where(p => p.HomePlayer1Id == player.Id || p.HomePlayer2Id == player.Id).Sum(s => s.HomeTeamScore.GetValueOrDefault())
                                         + playedMatchesOfTeam.Where(p => p.GuestPlayer1Id == player.Id || p.GuestPlayer2Id == player.Id).Sum(s => s.GuestTeamScore.GetValueOrDefault());

                tableEntry.GoalsConceded = playedMatchesOfTeam.Where(p => p.HomePlayer1Id == player.Id || p.HomePlayer2Id == player.Id).Sum(s => s.GuestTeamScore.GetValueOrDefault()) +
                                           playedMatchesOfTeam.Where(p => p.GuestPlayer1Id == player.Id || p.GuestPlayer2Id == player.Id).Sum(s => s.HomeTeamScore.GetValueOrDefault());

                tableEntry.Points = (tableEntry.GamesWon * PointsWin) + (tableEntry.GamesDraw * PointsDraw);
                db.Update(tableEntry, p => p.Id == tableEntry.Id);
            }
        }

        private static void EnsureEachPlayerHasTableEntry(IDbConnection db, List<UserAuth> players, List<CompetitionPlayerStanding> standings, int finalDayCompetitionId)
        {
            foreach (var player in players)
            {
                var problems = standings.Where(x => x.PlayerId == player.Id).ToList();
                var competitionPlayerStanding = standings.SingleOrDefault(s => s.PlayerId == player.Id);

                if (competitionPlayerStanding == null)
                {
                    competitionPlayerStanding = new CompetitionPlayerStanding { PlayerId = player.Id, FinalDayCompetitionId = finalDayCompetitionId };
                    competitionPlayerStanding.Id = (int)db.Insert(competitionPlayerStanding, true);
                }
            }
        }
    }
}