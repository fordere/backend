using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Cup
{
    [Route("/cups/{Id}/rounds", Verbs = "POST", Summary = "Creates a new cup round.")]
    
    public class CreateCupRoundRequest : IReturn<CupDto>
    {
        public int Id { get; set; }
    }
}