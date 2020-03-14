using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Competition
{
    [Route("/competitions/season/current", "GET", Summary = "Gets all competitions in current season")]
    
    public class GetAllCompetitionsInCurrentRequest : IReturn<List<CompetitionDto>>
    {
    }
}