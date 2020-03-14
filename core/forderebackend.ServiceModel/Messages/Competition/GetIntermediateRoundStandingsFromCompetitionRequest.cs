

using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Competition
{
    [Route("/competitions/{Id}/intermediateRoundStandings", "GET", Summary = "Get all ranking tables of a competition.")]
    
    public class GetIntermediateRoundStandingsFromCompetitionRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}