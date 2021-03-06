using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/group", "POST", Summary = "Add a Group to the given FinalDay Competition")]
    public class AddGroupRequest : IReturn<GroupDto>
    {
        public int FinalDayCompetitionId { get; set; }
    }
}