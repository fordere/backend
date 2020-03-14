using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/leagues/{LeagueId}/open-matches/usermails", "GET",
        Summary = "Gets all player emails for all open matches in a league.")]
    public class GetUserMailsForOpenMatchesInLeagueRequest : IReturn<UserMailsDto>
    {
        public int LeagueId { get; set; }
    }
}