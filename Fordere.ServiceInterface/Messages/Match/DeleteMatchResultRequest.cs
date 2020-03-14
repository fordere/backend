using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/{MatchId}/result", "DELETE")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DeleteMatchResultRequest : IReturn<ExtendedMatchDto>
    {
        public int MatchId { get; set; }
    }
}