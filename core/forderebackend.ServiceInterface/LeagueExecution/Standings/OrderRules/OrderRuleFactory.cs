using System.Collections.Generic;
using System.Data;
using System.Linq;

using Fordere.RestService.Entities;

using ServiceStack.OrmLite;

namespace Fordere.RestService.LeagueExecution.Standings.OrderRules
{
    public static class OrderRuleFactory
    {
        private const int NumberOfPoints = 1;
        private const int GoalDifference = 2;
        private const int MatchResult = 3;
        private const int NumberOfMatchesWon = 4;
        private const int NumberOfSetsWon = 5;
        private const int MatchNotPlayed = 6;

        public static List<IOrderRule> CreateOrderRules(IDbConnection db, int leagueId)
        {
            var league = db.SingleById<League>(leagueId);

            // TODO We should create new leagues with a DefaultStandingOrder
            var orderRuleIdentifiers = (league.StandingsOrder ?? "1,2,3,4,5").Split(',').Select(int.Parse);

            var orderRules = new List<IOrderRule>();

            foreach (var orderRuleIdentifier in orderRuleIdentifiers)
            {
                switch (orderRuleIdentifier)
                {
                    case NumberOfPoints:
                        orderRules.Add(new NumberOfPointsOrderRule());
                        break;
                    case GoalDifference:
                        orderRules.Add(new GoalDifferenceOrderRule());
                        break;
                    case MatchResult:
                        orderRules.Add(new MatchResultOrderRule(db));
                        break;
                    case NumberOfMatchesWon:
                        orderRules.Add(new NumberOfMatchsWonOrderRule());
                        break;
                    case NumberOfSetsWon:
                        orderRules.Add(new NumberOfSetsWonOrderRule());
                        break;
                    case MatchNotPlayed:
                        orderRules.Add(new MatchNotPlayedOrderRule());
                        break;
                }
            }

            return orderRules;
        }
    }
}