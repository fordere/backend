using Fordere.RestService.Sms;
using Fordere.ServiceInterface.Messages.Sms;

using ServiceStack;

namespace Fordere.RestService
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
