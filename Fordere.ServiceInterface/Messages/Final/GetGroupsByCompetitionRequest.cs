using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/group", "GET", Summary = "Gets all groups in a FinalDayCompetition")]
    public class GetGroupsByCompetitionRequest : IReturn<List<GroupDto>>
    {
        public int FinalDayCompetitionId { get; set; }
    }
}