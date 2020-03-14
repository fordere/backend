
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/{MatchId}/result", "DELETE")]
    
    public class DeleteMatchResultRequest : IReturn<ExtendedMatchDto>
    {
        public int MatchId { get; set; }
    }
}