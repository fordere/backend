using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/matches/myday", "GET", Summary = "Todays matches of the signed in user")]
    public class MyMatchesTodayRequest : IReturn<List<ExtendedMatchDto>>
    {
    }
}