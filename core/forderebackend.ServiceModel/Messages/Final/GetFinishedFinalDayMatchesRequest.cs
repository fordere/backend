using forderebackend.ServiceModel.Messages.User;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/{FinalDayId}/matches/finished", "GET")]
    public class GetFinishedFinalDayMatchesRequest : PagedRequest, IReturn<MatchesResponse>
    {
        public int FinalDayId { get; set; }

        public string TeamFilter { get; set; }

    }
}