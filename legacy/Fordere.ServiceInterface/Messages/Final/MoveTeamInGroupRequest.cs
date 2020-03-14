using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
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