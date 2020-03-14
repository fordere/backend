using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Competition
{
    [Route("/seasons/{SeasonId}/standings", "GET", Summary = "Get all ranking tables of a season.")]
    
    public class GetStandingsFromSeasonRequest : IReturn<List<CompetitionDto>>
    {
        public int SeasonId { get; set; }
    }
}