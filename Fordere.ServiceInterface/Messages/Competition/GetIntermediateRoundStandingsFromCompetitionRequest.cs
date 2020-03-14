using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/{Id}/intermediateRoundStandings", "GET", Summary = "Get all ranking tables of a competition.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetIntermediateRoundStandingsFromCompetitionRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}