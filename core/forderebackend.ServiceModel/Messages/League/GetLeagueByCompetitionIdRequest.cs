using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.League
{
    [Route("/competitions/{CompetitionId}/leagues", "GET", Summary = "Get leagues by competition.")]
    public class GetLeagueByCompetitionIdRequest : IReturn<LeagueDto>
    {
        public int CompetitionId { get; set; }
    }
}