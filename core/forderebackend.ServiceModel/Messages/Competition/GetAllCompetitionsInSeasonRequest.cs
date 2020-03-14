using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/season/{SeasonId}", "GET", Summary = "Gets all competitions in a season")]
    
    public class GetAllCompetitionsInSeasonRequest : IReturn<List<CompetitionDto>>
    {
        public int SeasonId { get; set; }
    }
}