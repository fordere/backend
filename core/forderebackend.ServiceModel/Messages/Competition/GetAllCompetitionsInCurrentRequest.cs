using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/season/current", "GET", Summary = "Gets all competitions in current season")]
    
    public class GetAllCompetitionsInCurrentRequest : IReturn<List<CompetitionDto>>
    {
    }
}