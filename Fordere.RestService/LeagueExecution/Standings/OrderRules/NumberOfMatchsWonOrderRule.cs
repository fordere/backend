using Fordere.RestService.Entities;

namespace Fordere.RestService.LeagueExecution.Standings.OrderRules
{
    public class NumberOfMatchsWonOrderRule : IOrderRule
    {
        public int Compare(TableEntry x, TableEntry y)
        {
            if (x.GamesWon > y.GamesWon)
            {
                return 1;
            }

            if (y.GamesWon > x.GamesWon)
            {
                return -1;
            }

            if (x.GamesWonExtension > y.GamesWonExtension)
            {
                return 1;
            }

            if (y.GamesWonExtension > x.GamesWonExtension)
            {
                return -1;
            }

            return 0;
        }
    }
}