using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/payments/done", "GET", Summary = "Gets all users which do not yet have payed for the current season")]
    public class GetDonePayments : IReturn<UsersResponse>
    {
        public int SeasonId { get; set; }

        public string Filter { get; set; }
    }
}