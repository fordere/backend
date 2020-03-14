using System.Collections.Generic;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/payments/open/user", "GET", Summary = "Gets all open payments for the current user")]
    public class GetUserOpenPaymentsForCurrentSeason : IReturn<List<OpenUserPaymentResponse>>
    {
    }
}