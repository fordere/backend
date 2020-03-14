

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/{Id}/intermediateRoundStandings", "GET", Summary = "Get all ranking tables of a competition.")]
    
    public class GetIntermediateRoundStandingsFromCompetitionRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}