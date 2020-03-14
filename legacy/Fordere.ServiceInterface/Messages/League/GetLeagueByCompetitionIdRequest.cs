using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.League
{
    [Route("/competitions/{CompetitionId}/leagues", "GET", Summary = "Get leagues by competition.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetLeagueByCompetitionIdRequest : IReturn<LeagueDto>
    {
        public int CompetitionId { get; set; }
    }
}