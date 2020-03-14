using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.MailingList
{
    [Route("/mailinglist/allusers", "GET", Summary = "Gets all mails of all users on fordere.ch")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllUsersMailsRequest : IReturn<UserMailsDto>
    {

    }
}