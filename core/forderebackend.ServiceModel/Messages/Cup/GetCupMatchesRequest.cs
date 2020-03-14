using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Cup
{
    [Route("/cups/{Id}/matches", "GET")]
    [Route("/cups/{Id}/round/{CupRound}", "GET")]
    public class GetCupMatchesRequest : IReturn<ExtendedMatchDto>
    {
        public int Id { get; set; }
        public int? CupRound { get; set; }
    }
}