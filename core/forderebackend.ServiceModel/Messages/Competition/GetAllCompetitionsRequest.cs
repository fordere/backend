using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Competition
{
    [Route("/competitions", "GET", Summary = "Gets all competitions")]
    
    public class GetAllCompetitionsRequest : IReturn<List<CompetitionDto>>
    {
    }
}