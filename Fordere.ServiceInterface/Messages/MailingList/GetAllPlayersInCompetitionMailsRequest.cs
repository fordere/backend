using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.MailingList
{
    [Route("/mailinglist/competition/{CompetitionId}", "GET", Summary = "Gets all mails of all players in a competition")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllPlayersInCompetitionMailsRequest : IReturn<UserMailsDto>
    {
        public int CompetitionId { get; set; }
    }
}