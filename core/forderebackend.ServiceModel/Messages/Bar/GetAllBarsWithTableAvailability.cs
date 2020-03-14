using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Bar
{
    [Route("/bars/playable", "GET",
        Summary = "Get all bars which do have a table availability and therfore can be played in")]
    public class GetAllBarsWithTableAvailability : IReturn<List<BarDto>>
    {
    }
}