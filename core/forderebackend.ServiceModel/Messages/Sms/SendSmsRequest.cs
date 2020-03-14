using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Sms
{
    [Route("/sms", "POST", Summary = "Sends an SMS")]
    public class SendSmsRequest : IReturnVoid
    {
        public string Number { get; set; }

        public string Text { get; set; }
    }
}