using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/payments", "POST", Summary = "Does save the payment-state for a given user")]
    public class SavePayment : IReturnVoid
    {
        public int Id { get; set; }

        public bool HasPaid { get; set; }

        public string Comment { get; set; }
    }
}