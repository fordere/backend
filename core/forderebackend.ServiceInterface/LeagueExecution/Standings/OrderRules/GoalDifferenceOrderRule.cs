using Fordere.RestService.Entities;

namespace Fordere.RestService.LeagueExecution.Standings.OrderRules
{
    public class GoalDifferenceOrderRule : IOrderRule
    {
        public int Compare(TableEntry x, TableEntry y)
        {
            if (x.PlusMinus > y.PlusMinus)
            {
                return 1;
            }

            if (y.PlusMinus > x.PlusMinus)
            {
                return -1;
            }

            return 0;
        }
    }
}