using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/matches/{MatchId}/result", "DELETE")]
    
    public class DeleteMatchResultRequest : IReturn<ExtendedMatchDto>
    {
        public int MatchId { get; set; }
    }
}