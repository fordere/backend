using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.MailingList
{
    [Route("/mailinglist/league/{LeagueId}", "GET", Summary = "Gets all mails of all players in a league")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllPlayersInLeagueMailsRequest : IReturn<UserMailsDto>
    {
        public int LeagueId { get; set; }
    }
}