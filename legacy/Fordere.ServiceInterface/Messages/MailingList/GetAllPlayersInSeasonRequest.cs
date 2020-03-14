using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.MailingList
{
    [Route("/mailinglist/season/{SeasonId}", "GET", Summary = "Gets all mails of all players in a season")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllPlayersInSeasonMailsRequest : IReturn<UserMailsDto>
    {
        public int SeasonId { get; set; }
    }
}
