using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Cup
{
    [Route("/cups/{Id}/create/{CompetitionId}", "GET", Summary = "Generates all firstround matches for a cup.")]
    public class CreateCupFromLeagueRequest : IReturn<CupDto>
    {
        public int Id { get; set; }

        public int CompetitionId { get; set; }
    }
}