using Fordere.ServiceInterface.Messages.User;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/{FinalDayId}/matches/recent", "GET")]
    public class GetRecentFinalDayMatchesRequest : PagedRequest, IReturn<MatchesResponse>
    {
        public int FinalDayId { get; set; }     
    }
}