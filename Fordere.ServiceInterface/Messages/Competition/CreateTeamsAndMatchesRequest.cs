using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/{CompetitionId}/generate", "POST", Summary = "Generates all Teams & Matches for a competition. Existing Teams/Matches will be deleted.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CreateTeamsAndMatchesRequest : IReturnVoid
    {
        public int CompetitionId { get; set; }
    }
}