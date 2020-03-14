using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/group/moveteam/{TeamInGroupId}", "POST", Summary = "Moves a TeamInGroup (Settlement)")]
    public class MoveTeamInGroupRequest : IReturn<List<TeamInGroupViewDto>>
    {
        public int FinalDayCompetitionId { get; set; }

        public int TeamInGroupId { get; set; }

        public int TargetGroupId { get; set; }

        public int TargetSettlement { get; set; }
    }
}