using System.Collections.Generic;
using System.Threading.Tasks;

using Fordere.RestService.Entities;
using Fordere.RestService.Sms;
using Fordere.RestService.Smtp;

using ServiceStack.Auth;

namespace Fordere.RestService
{
    public class PlayerContacter
    {
        public async Task SendRecall(List<UserAuth> users, Match match)
        {
            foreach (var user in users)
            {
                if (HasValidMobileNumber(user))
                {
                    await new SmsSender().SendMatchRecall(user, match);
                }
                else if (HasValidMail(user))
                {
                    MailSender.SendMatchRecall(user.MailAddress, match);
                }
            }
        }

        public async Task SendMatchAssigned(List<UserAuth> users, Match match)
        {
            foreach (var user in users)
            {
                if (HasValidMobileNumber(user))
                {
                    await new SmsSender().SendMatchAssigned(user, match);
                }
                else if (HasValidMail(user))
                {
                    MailSender.SendMatchAssigend(user.MailAddress, match);
                }
            }
        }

        private static bool HasValidMail(UserAuth user)
        {
            // TODO SSH: könnten wir noch machen... 
            return false;
        }

        private static bool HasValidMobileNumber(UserAuth user)
        {
            // TODO SSH is this check valid?
            string formattedNumber = PhoneNumberFormatter.Format(user.PhoneNumber);
            return formattedNumber.Length == 12 && formattedNumber.StartsWith("+");
        }
    }
}