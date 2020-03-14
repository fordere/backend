

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/{Id}/standings", "GET", Summary = "Get all ranking tables of a competition.")]
    
    public class GetStandingsFromCompetitionsRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}