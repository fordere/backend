using ServiceStack;

namespace forderebackend.ServiceModel.Messages
{
    [Route("/contact", "POST", Summary = "Contact the fordere")]
    public class ContactRequest : IReturnVoid, ICaptchaRequest
    {
        public string Mail { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string Captcha { get; set; }

    }
}