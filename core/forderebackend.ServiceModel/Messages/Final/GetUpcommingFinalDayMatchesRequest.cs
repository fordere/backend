using forderebackend.ServiceModel.Messages.User;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/{FinalDayId}/matches/upcomming", "GET")]
    public class GetUpcommingFinalDayMatchesRequest : PagedRequest, IReturn<MatchesResponse>
    {
        public int FinalDayId { get; set; }
    }
}