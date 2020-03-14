using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Cup
{
    [Route("/cups/{Id}/matches", "GET")]
    [Route("/cups/{Id}/round/{CupRound}", "GET")]
    public class GetCupMatchesRequest : IReturn<ExtendedMatchDto>
    {
        public int Id { get; set; }
        public int? CupRound { get; set; }
    }
}