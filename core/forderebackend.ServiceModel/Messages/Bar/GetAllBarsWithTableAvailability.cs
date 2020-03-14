using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Bar
{
    [Route("/bars/playable", "GET", Summary = "Get all bars which do have a table availability and therfore can be played in")]
    
    public class GetAllBarsWithTableAvailability : IReturn<List<BarDto>>
    {
    }
}