using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/users/possiblepartner", "GET", Summary = "Get all users which are possible partners for the current user for the given competition")]
    public class GetAllPossiblePartners : IReturn<UsersResponse>
    {
        public int CompetitionId { get; set; }
    }
}