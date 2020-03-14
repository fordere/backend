using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/{FinalDayId}/matches/running", "GET")]
    public class GetRunningFinalDayMatchesRequest : IReturn<MatchDto>
    {
        public int FinalDayId { get; set; }
    }
}