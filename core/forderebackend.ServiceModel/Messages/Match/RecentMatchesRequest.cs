using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/matches/recent", "GET", Summary = "Recent played matches (7 days).")]
    
    public class RecentMatchesRequest : IReturn<List<ExtendedMatchDto>>
    {
    }
}