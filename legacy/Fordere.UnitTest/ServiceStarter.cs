using Fordere.WebConsole;

using ServiceStack;

namespace Fordere.UnitTest
{
    public class ServiceStarter
    {
        public const string ServiceUrl = "http://localhost:10000/";

        private static readonly object locker = new object();

        public static void EnsureServiceIsRunning()
        {
            //if (!EnvironmentHelper.IsAdministrator())
            //{
            //    Assert.Fail("Run VisualStudio as administrator");
            //}

            lock (locker)
            {
                if (ServiceStackHost.Instance == null)
                {
                    //var appHost = new UnitTestAppHost();
                    var appHost = new AppHostConsole();
                    appHost.Init();
                    appHost.Start(ServiceUrl);
                }
            }
        }
    }
}
