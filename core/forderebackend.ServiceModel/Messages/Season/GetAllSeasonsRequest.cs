using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Season
{
    [Route("/seasons", "GET", Summary = "Get all seasons.")]
    
    public class GetAllSeasonsRequest : IReturn<List<SeasonDto>>
    {
    }
}