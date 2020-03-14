
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Cup
{
    [Route("/cups/{Id}/rounds", Verbs = "POST", Summary = "Creates a new cup round.")]
    
    public class CreateCupRoundRequest : IReturn<CupDto>
    {
        public int Id { get; set; }
    }
}