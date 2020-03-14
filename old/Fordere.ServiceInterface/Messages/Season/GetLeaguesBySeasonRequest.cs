using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Season
{
    [Route("/seasons/{SeasonId}/leagues", "GET", Summary = "Get leagues by season")]
    public class GetLeaguesBySeasonRequest : IReturn<List<LeagueDto>>
    {
        public int SeasonId { get; set; }
    }
}