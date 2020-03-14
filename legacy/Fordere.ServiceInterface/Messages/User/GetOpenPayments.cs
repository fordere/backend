using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/payments/open", "GET", Summary = "Gets all users which have already payed for the current season")]
    public class GetOpenPayments : IReturn<UsersResponse>
    {
        public int SeasonId { get; set; }

        public string Filter { get; set; }
    }
}