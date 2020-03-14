using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/leagues/{LeagueId}/open-matches/usermails", "GET", Summary = "Gets all player emails for all open matches in a league.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetUserMailsForOpenMatchesInLeagueRequest : IReturn<UserMailsDto>
    {
        public int LeagueId { get; set; }
    }
}