using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/{Id}/reset", "POST")]
    public class ResetMatchRequest : IReturn<MatchViewDto>
    {
        public int Id { get; set; }
    }
}