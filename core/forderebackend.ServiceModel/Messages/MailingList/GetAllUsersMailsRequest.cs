
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.MailingList
{
    [Route("/mailinglist/allusers", "GET", Summary = "Gets all mails of all users on fordere.ch")]
    
    public class GetAllUsersMailsRequest : IReturn<UserMailsDto>
    {

    }
}