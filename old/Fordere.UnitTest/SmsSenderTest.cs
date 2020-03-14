using Fordere.RestService.Sms;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fordere.UnitTest
{
    [TestClass]
    public class SmsSenderTest
    {
        [TestMethod]
        public void Send()
        {
            new SmsSender().SendSmsAsync("+41797532816", "Hallo Welt");
        }
    }
}