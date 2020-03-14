using Fordere.RestService.Entities;

namespace Fordere.RestService.LeagueExecution.Standings.OrderRules
{
    public class NumberOfPointsOrderRule : IOrderRule
    {
        public int Compare(TableEntry x, TableEntry y)
        {
            if (x.Points > y.Points)
            {
                return 1;
            }

            if (y.Points > x.Points)
            {
                return -1;
            }

            return 0;
        }
    }
}
