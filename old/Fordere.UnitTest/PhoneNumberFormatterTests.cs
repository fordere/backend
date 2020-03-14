using Fordere.RestService.Sms;

using NUnit.Framework;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Fordere.UnitTest
{
    [TestFixture]
    public class PhoneNumberFormatterTests
    {
        [TestCase("079 753 28 16")]
        [TestCase("0797532816")]
        [TestCase("079 753      28 16")]
        [TestCase("+41 79 753 28 16")]
        [TestCase("+41 (0) 79 753      28 16")]
        [TestCase("0041 79 753 28 16")]
        public void Test1(string input)
        {
            var output = PhoneNumberFormatter.Format(input);
            Assert.AreEqual("+41797532816", output);
        }
    }
}
