using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Season
{
    [Route("/seasons/{SeasonId}/teams", "GET", Summary = "Get all teams in a season")]
    public class GetAllTeamsInSeasonRequest : IReturn<List<TeamDto>>
    {
        public int SeasonId { get; set; }
    }
}