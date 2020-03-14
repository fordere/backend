using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/team/{TeamId}", "GET")]
    public class GetMatchesForTeamRequest : IReturn<List<ExtendedMatchDto>>
    {
        public int TeamId { get; set; }
    }
}