using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.LeagueRegistration
{
    [UsedImplicitly]
    [Route("/competition/open", "GET", Summary = "Get competitions ready for registrations")]
    public class GetCompetitionsOpenForRegistration : IReturn<List<CompetitionDto>>
    {
    }
}