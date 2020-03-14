using ServiceStack;

namespace Fordere.UnitTest
{
    public class ClientHelper
    {
        public static JsonServiceClient GetClient()
        {
            var client = new JsonServiceClient(ServiceStarter.ServiceUrl);
            client.SetCredentials("oliver.zuercher@gmail.com", "blub");
            client.AlwaysSendBasicAuthHeader = true;
            
            return client;
        }
    }
}