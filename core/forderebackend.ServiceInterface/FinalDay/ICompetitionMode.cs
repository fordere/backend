using System.Collections.Generic;

using Fordere.RestService.Entities;

namespace Fordere.RestService.FinalDay
{
    public interface ICompetitionMode
    {
        List<Match> GenerateMatches(int finalDayCompetitionId);

        List<Match> GenerateMatchAfterMatchResultEntered(Match match);

        void AfterMatchSafe(List<Match> dayCompetitionId, int finalDayCompetitionId);

        long GetNumberOfMatches(int finalDayCompetitionId);

        bool ShouldFinishCompetitionWhenNoMoreMatchesOpen();
    }
}