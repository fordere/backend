using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/{CompetitionId}/state", "GET", Summary = "Returns the state of a competition")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetCompetitionStateRequest : IReturn<CompetitionStateDto>
    {
        public int CompetitionId { get; set; }
    }


}