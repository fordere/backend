using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    // TODO ssh: Check api
    [Route("/finalday/{FinalDayId}/finished", "GET", Summary = "Get's all finished FinalDayCompetitions of a season")]
    public class GetFinishedFinalDayCompetitions : IReturn<List<FinalDayCompetitionDto>>
    {
        public int FinalDayId { get; set; }

        public CompetitionMode CompetitionMode { get; set; }
    }
}