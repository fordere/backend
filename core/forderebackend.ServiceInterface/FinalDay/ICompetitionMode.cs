using System.Collections.Generic;
using forderebackend.ServiceInterface.Entities;

namespace forderebackend.ServiceInterface.FinalDay
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