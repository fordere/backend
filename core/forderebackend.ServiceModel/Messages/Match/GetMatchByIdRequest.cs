using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/matches/{Id}", "GET", Summary = "Get match by Id.")]
    public class GetMatchByIdRequest : IReturn<ExtendedMatchDto>
    {
        public int Id { get; set; }
    }
}