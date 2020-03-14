using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Season
{
    [Route("/seasons/archived", "GET", Summary = "Get all seasons.")]
    
    public class GetAllArchivedSeasonsRequest : IReturn<List<SeasonDto>>
    {
    }
}