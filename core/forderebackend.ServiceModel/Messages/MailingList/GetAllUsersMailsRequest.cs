using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.MailingList
{
    [Route("/mailinglist/allusers", "GET", Summary = "Gets all mails of all users on fordere.ch")]
    public class GetAllUsersMailsRequest : IReturn<UserMailsDto>
    {
    }
}