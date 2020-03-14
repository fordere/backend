using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Challenge
{
    [Route("/leagues/{LeagueId}/challenges", "GET")]
    [UsedImplicitly]
    public class GetChallengesForLeague : IReturn<List<ChallengeDto>>
    {
        public int LeagueId { get; set; }
    }
}