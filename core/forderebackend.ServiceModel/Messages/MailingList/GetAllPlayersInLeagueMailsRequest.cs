using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.MailingList
{
    [Route("/mailinglist/league/{LeagueId}", "GET", Summary = "Gets all mails of all players in a league")]
    public class GetAllPlayersInLeagueMailsRequest : IReturn<UserMailsDto>
    {
        public int LeagueId { get; set; }
    }
}