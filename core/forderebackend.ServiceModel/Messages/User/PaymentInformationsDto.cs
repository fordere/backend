namespace forderebackend.ServiceModel.Messages.User
{
    public class PaymentInformationsDto
    {
        public string PublicStripeKey { get; set; }

        public string BankInformations { get; set; }
    }
}