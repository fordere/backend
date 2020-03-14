using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Season
{
    [Route("/seasons", "GET", Summary = "Get all seasons.")]
    
    public class GetAllSeasonsRequest : IReturn<List<SeasonDto>>
    {
    }
}