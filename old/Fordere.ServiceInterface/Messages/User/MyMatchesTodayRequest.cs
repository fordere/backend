using System.Collections.Generic;
using Fordere.ServiceInterface.Dtos;
using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/matches/myday", "GET", Summary = "Todays matches of the signed in user")]
    public class MyMatchesTodayRequest : IReturn<List<ExtendedMatchDto>>
    {
    }
}