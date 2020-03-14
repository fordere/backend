using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/matches/{Id}/reset", "POST")]
    public class ResetMatchRequest : IReturn<MatchViewDto>
    {
        public int Id { get; set; }
    }
}