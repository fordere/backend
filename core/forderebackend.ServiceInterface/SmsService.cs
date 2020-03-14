using forderebackend.ServiceInterface.Sms;
using forderebackend.ServiceModel.Messages.Sms;
using ServiceStack;

namespace forderebackend.ServiceInterface
{
    public class SmsService : BaseService
    {
        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public async void Post(SendSmsRequest request)
        {
            var result = await new SmsSender().SendSmsAsync(request.Number, request.Text);
        }
    }
}