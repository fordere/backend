using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/{Id}/teams", "GET", Summary = "Get all teams of a competition.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetTeamsFromCompetitionsRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}