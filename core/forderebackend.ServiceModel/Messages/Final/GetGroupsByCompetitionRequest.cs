using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/group", "GET", Summary = "Gets all groups in a FinalDayCompetition")]
    public class GetGroupsByCompetitionRequest : IReturn<List<GroupDto>>
    {
        public int FinalDayCompetitionId { get; set; }
    }
}