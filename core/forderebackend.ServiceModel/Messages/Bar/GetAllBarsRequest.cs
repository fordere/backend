using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Bar
{
    [Route("/bars", "GET", Summary = "Get all bars.")]
    
    public class GetAllBarsRequest : IReturn<List<BarDto>>
    {
    }
}