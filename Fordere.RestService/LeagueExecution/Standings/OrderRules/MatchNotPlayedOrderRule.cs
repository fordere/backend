using Fordere.RestService.Entities;

namespace Fordere.RestService.LeagueExecution.Standings.OrderRules
{
    public class MatchNotPlayedOrderRule : IOrderRule
    {
        public int Compare(TableEntry x, TableEntry y)
        {
            if (x.Id == y.Id)
            {
                return 0;
            }

            // Both teams have not enough games played
            if (x.GamesNotPlayed > StandingsCalculator.MaxNumberOfNotPlayedMatches && y.GamesNotPlayed > StandingsCalculator.MaxNumberOfNotPlayedMatches)
            {
                return 0;
            }

            // Both teams have enough games played
            if (x.GamesNotPlayed <= StandingsCalculator.MaxNumberOfNotPlayedMatches && y.GamesNotPlayed <= StandingsCalculator.MaxNumberOfNotPlayedMatches)
            {
                return 0;
            }

            // X has too many not played matches
            if (x.GamesNotPlayed > StandingsCalculator.MaxNumberOfNotPlayedMatches && y.GamesNotPlayed <= StandingsCalculator.MaxNumberOfNotPlayedMatches)
            {
                return -1;
            }

            // y has too many not played matches
            return 1;
        }
    }
}