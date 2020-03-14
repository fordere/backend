using System.Collections.Generic;
using System.Net.Http;

namespace forderebackend.ServiceInterface
{
    public static class CaptchaSolver
    {
        public static bool Solve(string userIp, string response)
        {
            var client = new HttpClient();

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"privatekey", "TODO"},
                {"remoteip", userIp},
                {"response", response}
            });

            var validationResponsee = client.PostAsync("http://www.google.com/recaptcha/api/verify", content).Result;

            var stringResponse = validationResponsee.Content.ReadAsStringAsync().Result;

            var responseParts = stringResponse.Split('\n');

            if (responseParts.Length < 2)
            {
                return false;
            }

            return bool.Parse(responseParts[0]);
        }
    }
}