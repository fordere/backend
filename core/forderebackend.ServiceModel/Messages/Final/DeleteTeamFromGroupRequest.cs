using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/group/deleteteam/{TeamInGroupId}", "DELETE", Summary = "Delets a team out of a group")]
    public class DeleteTeamFromGroupRequest : IReturn<List<TeamInGroupViewDto>>
    {
        public int FinalDayCompetitionId { get; set; }

        public int TeamInGroupId { get; set; }
    }
}