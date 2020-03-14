using System.Collections.Generic;
using System.Threading.Tasks;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Sms;
using forderebackend.ServiceInterface.Smtp;
using ServiceStack.Auth;

namespace forderebackend.ServiceInterface
{
    public class PlayerContacter
    {
        public async Task SendRecall(List<UserAuth> users, Match match)
        {
            foreach (var user in users)
                if (HasValidMobileNumber(user))
                    await new SmsSender().SendMatchRecall(user, match);
                else if (HasValidMail(user)) MailSender.SendMatchRecall(user.MailAddress, match);
        }

        public async Task SendMatchAssigned(List<UserAuth> users, Match match)
        {
            foreach (var user in users)
                if (HasValidMobileNumber(user))
                    await new SmsSender().SendMatchAssigned(user, match);
                else if (HasValidMail(user)) MailSender.SendMatchAssigend(user.MailAddress, match);
        }

        private static bool HasValidMail(UserAuth user)
        {
            // TODO SSH: könnten wir noch machen... 
            return false;
        }

        private static bool HasValidMobileNumber(UserAuth user)
        {
            // TODO SSH is this check valid?
            var formattedNumber = PhoneNumberFormatter.Format(user.PhoneNumber);
            return formattedNumber.Length == 12 && formattedNumber.StartsWith("+");
        }
    }
}