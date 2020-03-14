using System.Collections.Generic;
using System.Data;
using System.Linq;

using Fordere.RestService.Entities;

using ServiceStack.OrmLite;

namespace Fordere.RestService.LeagueExecution.Standings.OrderRules
{
    public class MatchResultOrderRule : IOrderRule
    {
        private readonly IDbConnection db;

        public MatchResultOrderRule(IDbConnection db)
        {
            this.db = db;
        }

        public int Compare(TableEntry x, TableEntry y)
        {
            if (x.TeamId == y.TeamId)
            {
                return 0;
            }

            var matches = db.LoadSelect<Match>(sql => (sql.LeagueId == x.LeagueId && ((sql.HomeTeamId == x.TeamId && sql.GuestTeamId == y.TeamId) || (sql.HomeTeamId == y.TeamId && sql.GuestTeamId == x.TeamId)))).ToList();
            if (matches.Count == 1)
            {
                var match = matches.Single();
                if (match.WinnerTeamId == x.TeamId)
                {
                    return 1;
                }

                if (match.WinnerTeamId == y.TeamId)
                {
                    return -1;
                }

                return 0;
            }

            if (HasMoreWins(x.TeamId, matches))
            {
                return 1;
            }

            if (HasMoreWins(y.TeamId, matches))
            {
                return -1;
            }

            return 0;

        }

        private bool HasMoreWins(int teamId, List<Match> matches)
        {
            var wins = new Dictionary<int, int>();
            foreach (var match in matches)
            {
                if (wins.ContainsKey(match.WinnerTeamId))
                {
                    wins[match.WinnerTeamId]++;
                }
                else
                {
                    wins.Add(match.WinnerTeamId, 1);
                }
            }

            return wins.Aggregate((l, r) => l.Value > r.Value ? l : r).Key == teamId;
        }
    }
}