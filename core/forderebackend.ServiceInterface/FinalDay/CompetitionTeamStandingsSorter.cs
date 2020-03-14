using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Final;

namespace forderebackend.ServiceInterface.FinalDay
{
    public static class CompetitionTeamStandingsSorter
    {
        public static IOrderedEnumerable<CompetitionTeamStanding> OrderByRanking(
            IList<CompetitionTeamStanding> standings, IList<Match> matches)
        {
            var matchResultComparer = new DirectMatchComparar(matches);
            var competitionTeamStandings =
                standings.OrderByDescending(p => p.Points).ThenByDescending(x => x, matchResultComparer);
            return competitionTeamStandings.ThenByDescending(o => o.PlusMinus);
        }

        public class DirectMatchComparar : IComparer<CompetitionTeamStanding>
        {
            private readonly IList<Match> matches;

            public DirectMatchComparar(IList<Match> matches)
            {
                this.matches = matches;
            }

            public int Compare(CompetitionTeamStanding x, CompetitionTeamStanding y)
            {
                if (x.TeamId == y.TeamId)
                {
                    return 0;
                }

                var match = matches.Single(sql =>
                    sql.HomeTeamId == x.TeamId && sql.GuestTeamId == y.TeamId ||
                    sql.HomeTeamId == y.TeamId && sql.GuestTeamId == x.TeamId);

                if (HasWinningCircle(match, match.LoserTeamId, new List<Match>()))
                {
                    return 0;
                }


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

            private bool HasWinningCircle(Match rootMatch, int loserTeamId, List<Match> visitedMatches)
            {
                var possibleCircleMatches =
                    matches.Where(x => x.WinnerTeamId == loserTeamId && !visitedMatches.Contains(x)).ToList();
                if (!possibleCircleMatches.Any())
                {
                    return false;
                }

                if (possibleCircleMatches.Any(x => x == rootMatch))
                {
                    return true;
                }

                foreach (var possibleCircleMatch in possibleCircleMatches)
                {
                    visitedMatches.Add(possibleCircleMatch);
                    if (HasWinningCircle(rootMatch, possibleCircleMatch.LoserTeamId, visitedMatches))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}