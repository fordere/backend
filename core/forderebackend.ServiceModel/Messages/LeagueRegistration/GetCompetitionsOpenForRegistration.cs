using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.LeagueRegistration
{
    [Route("/competition/open", "GET", Summary = "Get competitions ready for registrations")]
    public class GetCompetitionsOpenForRegistration : IReturn<List<CompetitionDto>>
    {
    }
}