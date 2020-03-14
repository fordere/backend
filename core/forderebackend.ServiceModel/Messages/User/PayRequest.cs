using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/payments/pay", "POST", Summary = "Executes the Stripe payment for the given user")]
    public class PayRequest : IReturnVoid
    {
        public string Token { get; set; }
        public int Amount { get; set; }
    }
}