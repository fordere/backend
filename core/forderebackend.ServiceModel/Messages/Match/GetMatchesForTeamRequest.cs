using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/matches/team/{TeamId}", "GET")]
    public class GetMatchesForTeamRequest : IReturn<List<ExtendedMatchDto>>
    {
        public int TeamId { get; set; }
    }
}