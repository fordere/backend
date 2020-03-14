using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Final;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.FinalDay
{
    public class UpcommingMatchSorter
    {
        class SortInfo
        {
            public MatchView Match { get; set; }

            public int PlayerOpenMatchesMax { get; set; }

            public int PlayerOpenMatchesSum { get; set; }

            public DateTime? LastPlayDate { get; set; }

            public bool HasFreeTable => NumberOfOpenTables > 0;

            public int NumberOfOpenTables { get; set; }
        }

        public List<MatchView> Sort(IDbConnection dbConnection, List<MatchView> matches, int finalDayId)
        {
            var tables = GetFreeTables(dbConnection, finalDayId);

            var userOpenGames = new Dictionary<int, int>();
            foreach (var match in matches)
            {
                CountPlayer(userOpenGames, match.HomePlayer1Id);
                CountPlayer(userOpenGames, match.HomePlayer2Id);
                CountPlayer(userOpenGames, match.GuestPlayer1Id);
                CountPlayer(userOpenGames, match.GuestPlayer2Id);
            }

            var sortInfos = matches.Select(x => new SortInfo { Match = x, NumberOfOpenTables = tables.Count(t => t.TableType == x.FinalDayTableType) }).ToList();

            foreach (var playerCounter in userOpenGames)
            {
                int playerId = playerCounter.Key;
                foreach (var matchView in matches.Where(matchView => IsPlayerInMatch(matchView, playerId)))
                {
                    var sortInfo = sortInfos.Single(x => x.Match.Id == matchView.Id);
                    sortInfo.PlayerOpenMatchesMax = Math.Max(playerCounter.Value, sortInfo.PlayerOpenMatchesMax);
                    sortInfo.PlayerOpenMatchesSum += playerCounter.Value;
                }
            }

            // TODO SSH Db request in dieser logik...  und performt diese Logik?
            var finalDayCompetitionIds = dbConnection.Select<FinalDayCompetition>(x => x.FinalDayId == finalDayId).Select(x => x.Id).ToList();
            var allPlayedMatches = dbConnection.Select<MatchView>(x => x.ResultDate != null && Sql.In(x.FinalDayCompetitionId, finalDayCompetitionIds)).ToList();
            foreach (var sortInfo in sortInfos)
            {
                sortInfo.LastPlayDate = allPlayedMatches.Where(
                    x =>
                        IsPlayerInMatch(x, sortInfo.Match.HomePlayer1Id) || IsPlayerInMatch(x, sortInfo.Match.HomePlayer2Id) || IsPlayerInMatch(x, sortInfo.Match.GuestPlayer1Id) ||
                        IsPlayerInMatch(x, sortInfo.Match.GuestPlayer2Id))
                    .Max(x => x.PlayDate) ?? new DateTime(1970, 1, 1);
            }

            var orderedSortInfos = sortInfos.OrderBy(x => x.Match.FinalDayCompetitionPriority).ThenByDescending(x => x.HasFreeTable).ThenByDescending(x => x.PlayerOpenMatchesMax).ThenByDescending(x => x.PlayerOpenMatchesSum).ThenBy(x => x.NumberOfOpenTables).ThenBy(x => x.LastPlayDate);
            var matchViews = orderedSortInfos.Select(x => x.Match).ToList();
            return matchViews;
        }

        private static List<FinalDayTable> GetFreeTables(IDbConnection dbConnection, int finalDayId)
        {
            var allTables = dbConnection.Select(dbConnection.From<FinalDayTable>().Where(x => x.FinalDayId == finalDayId));
            var finalDayCompetitionIds = dbConnection.Select(dbConnection.From<FinalDayCompetition>().Where(x => x.FinalDayId == finalDayId)).Select(x => x.Id).ToList();
            var runningMatches = dbConnection.Select(dbConnection.From<MatchView>().Where(x => x.ResultDate == null && x.PlayDate != null && Sql.In(x.FinalDayCompetitionId, finalDayCompetitionIds)));

            var occupiedTables = runningMatches.Select(x => x.FinalDayTableId);

            return allTables.Where(x => !occupiedTables.Any(t => t.Value == x.Id)).ToList();
        }

        private static bool IsPlayerInMatch(MatchView matchView, int playerId)
        {
            return matchView.HomePlayer1Id == playerId || matchView.HomePlayer2Id == playerId || matchView.GuestPlayer1Id == playerId || matchView.GuestPlayer2Id == playerId;
        }

        private static void CountPlayer(IDictionary<int, int> userOpenGames, int playerId)
        {
            if (userOpenGames.ContainsKey(playerId))
            {
                userOpenGames[playerId] = userOpenGames[playerId] + 1;
            }
            else
            {
                userOpenGames[playerId] = 1;
            }
        }
    }
}