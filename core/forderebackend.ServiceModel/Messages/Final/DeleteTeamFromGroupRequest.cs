using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/group/deleteteam/{TeamInGroupId}", "DELETE", Summary = "Delets a team out of a group")]
    public class DeleteTeamFromGroupRequest : IReturn<List<TeamInGroupViewDto>>
    {
        public int FinalDayCompetitionId { get; set; }

        public int TeamInGroupId { get; set; }
    }
}