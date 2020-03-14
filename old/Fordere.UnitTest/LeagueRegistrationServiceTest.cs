//using System;
//using System.Linq;
//using System.Net;

//using Fordere.ServiceInterface.Messages.LeagueRegistration;

//using Microsoft.VisualStudio.TestTools.UnitTesting;

//using ServiceStack;

//namespace Fordere.UnitTest
//{
//    [TestClass]
//    public class LeagueRegistrationServiceTest
//    {
//        [ClassInitialize]
//        public static void ClassInitialize(TestContext context)
//        {
//            ServiceStarter.EnsureServiceIsRunning();
//        }

//        [TestMethod]
//        public void CreateRegistrationTest()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new AddLeagueRegistration { CloseDate = DateTime.Now.AddDays(7), CountSubLeagues = 3, IsVisible = true, Name = "Test" };
//            var response = client.Post(request);

//            Assert.IsTrue(string.IsNullOrEmpty(response.Id) == false);

//            var reload = client.Get(new GetLeagueRegistrationById { Id = response.Id });

//            Assert.AreEqual(request.Name, reload.Name);
//            // RavenDB Datetime cuts 4 digits on ticks
//            Assert.AreEqual(request.CloseDate.Day, reload.CloseDate.Day);
//            Assert.AreEqual(request.CloseDate.Month, reload.CloseDate.Month);
//            Assert.AreEqual(request.CloseDate.Year, reload.CloseDate.Year);
//            Assert.AreEqual(request.CloseDate.Hour, reload.CloseDate.Hour);
//            Assert.AreEqual(request.CloseDate.Minute, reload.CloseDate.Minute);
//            Assert.AreEqual(request.CloseDate.Second, reload.CloseDate.Second);
//            Assert.AreEqual(request.CloseDate.Millisecond, reload.CloseDate.Millisecond);
//            Assert.AreEqual(request.CountSubLeagues, reload.CountSubLeagues);
//            Assert.AreEqual(request.IsVisible, reload.IsVisible);
//        }

//        [TestMethod]
//        public void GetOpenLeagueRegistrationsTest()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new GetOpenLeagueRegistrations();

//            var response = client.Get(request);

//            Assert.IsTrue(response.Any(p => p.Name == "Test Liga 2"));
//        }

//        [TestMethod]
//        public void RegisterTeamTest()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new AddTeamRegistration { LeagueRegistrationId = 3, LeagueWish = 1, UserIds = new[] { "users/1", "users/2" }, Name = "Tätsch Bäng Mereng" };

//            var response = client.Post(request);

//            Assert.AreEqual(request.Name, response.Name);
//            Assert.AreEqual(request.LeagueWish, response.LeagueWish);

//            var leagueRegistration = client.Get(new GetLeagueRegistrationById { Id = 3 });

//            Assert.IsTrue(leagueRegistration.TeamRegistrations.Any(p => p.Name == request.Name));
//        }

//        [TestMethod]
//        public void RegisterTeamToNonVisibleRegistration()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new AddTeamRegistration { LeagueRegistrationId = 5, LeagueWish = 1, UserIds = new[] { "users/1", "users/2" }, Name = "Tätsch Bäng Mereng" };

//            try
//            {
//                client.Post(request);
//                Assert.Fail("Request should return BadRequest.");
//            }
//            catch (WebServiceException exception)
//            {
//                Assert.AreEqual((int)HttpStatusCode.BadRequest, exception.StatusCode);
//            }
//        }

//        [TestMethod]
//        public void RegisterTeamToNotOpenRegistration()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new AddTeamRegistration { LeagueRegistrationId = 5, LeagueWish = 1, UserIds = new[] { "users/1", "users/2" }, Name = "Tätsch Bäng Mereng" };

//            try
//            {
//                client.Post(request);
//                Assert.Fail("Request should return BadRequest.");
//            }
//            catch (WebServiceException exception)
//            {
//                Assert.AreEqual((int)HttpStatusCode.BadRequest, exception.StatusCode);
//            }
//        }

//        [TestMethod]
//        public void RegisterAlreadyRegisteredTeamTest()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new AddTeamRegistration { LeagueRegistrationId = 4, LeagueWish = 1, UserIds = new[] { "users/1", "users/2" }, Name = "Tätsch Bäng Mereng" };

//            try
//            {
//                client.Post(request);
//                Assert.Fail("Request should return BadRequest.");
//            }
//            catch (WebServiceException exception)
//            {
//                Assert.AreEqual((int)HttpStatusCode.BadRequest, exception.StatusCode);
//            }
//        }

//        [TestMethod]
//        public void GetMyLeagueRegistrationsTest()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new GetMyLeagueRegistrations();

//            var response = client.Get(request);

//            Assert.AreEqual(1, response.Count);
//        }

//        [TestMethod]
//        public void GetLeagueRegistrationsOfUserTest()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new GetLeagueRegistrations { UserId = 1 };

//            var response = client.Get(request);

//            Assert.AreEqual(1, response.Count);
//        }
//    }
//}