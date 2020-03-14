using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Cup
{
    [Route("/cups", "GET", Summary = "Get all cups.")]
    public class GetAllCupsRequest : IReturn<List<Cup2Dto>>
    {
    }
}