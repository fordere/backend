using System;

using Fordere.ServiceInterface.Messages.Final;

namespace Fordere.ServiceInterface.Dtos.FinalDay
{
    public class FinalDayCompetitionProgressDto
    {
        public string CompetitionName { get; set; }

        public int CompetitionId { get; set; }

        public FinalDayCompetitionState CompetitionState { get; set; }

        public long MatchesPlayed { get; set; }

        public long MatchesOpen { get; set; }

        public long MatchesRunning { get; set; }

        public DateTime? ExpectedEnd { get; set; }

        public int AverageMatchDuration { get; set; }

        public CompetitionMode CompetitionMode { get; set; }
    }
}