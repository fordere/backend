﻿using Fordere.RestService.Entities;

namespace Fordere.RestService.LeagueExecution.Standings.OrderRules
{
    public class NumberOfSetsWonOrderRule : IOrderRule
    {
        public int Compare(TableEntry x, TableEntry y)
        {
            if (x.SetsWon > y.SetsWon)
            {
                return 1;
            }

            if (y.SetsWon > x.SetsWon)
            {
                return -1;
            }

            return 0;
        }
    }
}