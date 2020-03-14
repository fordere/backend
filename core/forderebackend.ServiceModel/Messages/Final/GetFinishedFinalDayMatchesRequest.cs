using Fordere.ServiceInterface.Messages.User;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/{FinalDayId}/matches/finished", "GET")]
    public class GetFinishedFinalDayMatchesRequest : PagedRequest, IReturn<MatchesResponse>
    {
        public int FinalDayId { get; set; }

        public string TeamFilter { get; set; }

    }
}