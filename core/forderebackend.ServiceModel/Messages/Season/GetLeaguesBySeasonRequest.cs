using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Season
{
    [Route("/seasons/{SeasonId}/leagues", "GET", Summary = "Get leagues by season")]
    public class GetLeaguesBySeasonRequest : IReturn<List<LeagueDto>>
    {
        public int SeasonId { get; set; }
    }
}