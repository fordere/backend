using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    // TODO ssh: Check api
    [Route("/finalday/{FinalDayId}/finished", "GET", Summary = "Get's all finished FinalDayCompetitions of a season")]
    public class GetFinishedFinalDayCompetitionsRequest : IReturn<List<FinalDayCompetitionDto>>
    {
        public int FinalDayId { get; set; }

        public CompetitionMode CompetitionMode { get; set; }
    }
}