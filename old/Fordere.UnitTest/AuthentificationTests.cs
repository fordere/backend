using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;

//using ServiceStack.OrmLite;

namespace Fordere.UnitTest
{
    [TestClass]
    public class AuthentificationTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ServiceStarter.EnsureServiceIsRunning();
        }

        [TestMethod]
        public void CredentialsAuthWithCookiesTest()
        {
            var client = new JsonServiceClient(ServiceStarter.ServiceUrl);
            
            var authResponse = client.Post(new Authenticate
            {
                UserName = "oliver.zuercher@gmail.com",
                Password = "blub",
                RememberMe = true,
            });

            Assert.IsNotNull(authResponse.SessionId, "SessionId must not be null after Authenticate request.");
        }

        
    }
}
