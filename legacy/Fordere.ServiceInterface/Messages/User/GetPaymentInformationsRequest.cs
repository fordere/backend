﻿using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/payments/informations", "GET", Summary = "Gets the payment-informations for the current division")]
    public class GetPaymentInformationsRequest : IReturn<PaymentInformationsDto>
    {
    }
}