using Fordere.ServiceInterface.Dtos;
using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Cup
{
    [Route("/cups/{Id}/create/{CompetitionId}", "GET", Summary = "Generates all firstround matches for a cup.")]
    public class CreateCupFromLeagueRequest : IReturn<CupDto>
    {
        public int Id { get; set; }

        public int CompetitionId { get; set; }
    }
}