using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/{Id}", "GET", Summary = "Get match by Id.")]
    public class GetMatchByIdRequest : IReturn<ExtendedMatchDto>
    {
        public int Id { get; set; }
    }
}