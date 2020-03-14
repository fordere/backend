

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/{Id}/teams", "GET", Summary = "Get all teams of a competition.")]
    
    public class GetTeamsFromCompetitionsRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}