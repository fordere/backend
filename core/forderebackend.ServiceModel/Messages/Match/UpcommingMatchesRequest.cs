using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/matches/upcomming", "GET", Summary = "Upcomming matches for the next 7 days.")]
    public class UpcommingMatchesRequest : IReturn<List<ExtendedMatchDto>>
    {
    }
}