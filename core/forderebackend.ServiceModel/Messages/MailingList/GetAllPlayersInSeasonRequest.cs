using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.MailingList
{
    [Route("/mailinglist/season/{SeasonId}", "GET", Summary = "Gets all mails of all players in a season")]
    
    public class GetAllPlayersInSeasonMailsRequest : IReturn<UserMailsDto>
    {
        public int SeasonId { get; set; }
    }
}
