using System.Threading.Tasks;
using forderebackend.ServiceInterface.Entities;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;

namespace forderebackend.ServiceInterface.Sms
{
    public class SmsSender
    {
        public async Task<string> SendSmsAsync(string number, string text)
        {
            var appSettings = new AppSettings();
            if (appSettings.Get<bool>("Sms.Enabled"))
            {
                var from = appSettings.GetString("Sms.From");
                var formattedNumber = PhoneNumberFormatter.Format(number);

                return await "http://api.clickatell.com/rest/message ".PostToUrlAsync(
                    "{\"text\":\"" + text + "\", \"to\":[\"" + formattedNumber + "\"], \"from\":\"" + from + "\"}",
                    "application/json", req =>
                    {
                        req.ContentType = "application/json";
                        req.Headers.Add("X-Version", "1");
                        var authToken = appSettings.GetString("Sms.Authorization.Token");
                        req.Headers.Add("Authorization", authToken);
                    });
            }

            return "";
        }

        public async Task<string> SendMatchRecall(UserAuth user, Match match)
        {
            var message = "RECALL! %FIRSTNAME%, du wirst an Tisch #%TABLENUMBER% vermisst!";
            message = message.Replace("%TABLENUMBER%", match.FinalDayTable.Number.ToString());
            message = message.Replace("%FIRSTNAME%", user.FirstName);

            return await SendSmsAsync(user.PhoneNumber, message);
        }

        public Task<string> SendMatchAssigned(UserAuth user, Match match)
        {
            var message = "Spiel an Tisch #%TABLENUMBER% gegen '%OPPONENT%'";
            message = message.Replace("%TABLENUMBER%", match.FinalDayTable.Number.ToString());

            string opponentTeam;
            if (match.HomeTeam.Player1Id == user.Id || match.HomeTeam.Player2Id == user.Id)
            {
                opponentTeam = match.GuestTeam.Name;
            }
            else
            {
                opponentTeam = match.HomeTeam.Name;
            }

            message = message.Replace("%OPPONENT%", opponentTeam);

            return SendSmsAsync(user.PhoneNumber, message);
        }
    }
}