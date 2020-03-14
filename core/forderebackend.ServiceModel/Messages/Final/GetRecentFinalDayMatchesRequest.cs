using forderebackend.ServiceModel.Messages.User;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/{FinalDayId}/matches/recent", "GET")]
    public class GetRecentFinalDayMatchesRequest : PagedRequest, IReturn<MatchesResponse>
    {
        public int FinalDayId { get; set; }     
    }
}