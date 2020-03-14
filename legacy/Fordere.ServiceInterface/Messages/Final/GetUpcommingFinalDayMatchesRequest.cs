using Fordere.ServiceInterface.Messages.User;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/{FinalDayId}/matches/upcomming", "GET")]
    public class GetUpcommingFinalDayMatchesRequest : PagedRequest, IReturn<MatchesResponse>
    {
        public int FinalDayId { get; set; }
    }
}