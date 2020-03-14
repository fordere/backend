using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Competition
{
    [Route("/competitions/{Id}/standings", "GET", Summary = "Get all ranking tables of a competition.")]
    public class GetStandingsFromCompetitionsRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}