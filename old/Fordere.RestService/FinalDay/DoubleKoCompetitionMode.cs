using System;
using System.Collections.Generic;

using Fordere.RestService.Entities;
using Fordere.RestService.Entities.Final;

namespace Fordere.RestService.FinalDay
{
    public class DoubleKoCompetitionMode : ICompetitionMode
    {
        public List<Match> GenerateMatches(int finalDayCompetitionId)
        {
            throw new NotImplementedException();
        }

        public List<Match> GenerateMatchAfterMatchResultEntered(Match match)
        {
            throw new NotImplementedException();
        }

        public void AfterMatchSafe(List<Match> dayCompetitionId, int finalDayCompetitionId)
        {
            throw new NotImplementedException();
        }

        public long GetNumberOfMatches(int finalDayCompetitionId)
        {
            throw new NotImplementedException();
        }

        public bool ShouldFinishCompetitionWhenNoMoreMatchesOpen()
        {
            return true;
        }
    }
}
