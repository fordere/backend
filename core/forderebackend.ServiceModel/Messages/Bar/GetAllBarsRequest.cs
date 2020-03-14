using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Bar
{
    [Route("/bars", "GET", Summary = "Get all bars.")]
    
    public class GetAllBarsRequest : IReturn<List<BarDto>>
    {
    }
}