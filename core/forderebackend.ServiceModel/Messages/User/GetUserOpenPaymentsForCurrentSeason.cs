using System.Collections.Generic;
using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/payments/open/user", "GET", Summary = "Gets all open payments for the current user")]
    public class GetUserOpenPaymentsForCurrentSeason : IReturn<List<OpenUserPaymentResponse>>
    {
    }
}