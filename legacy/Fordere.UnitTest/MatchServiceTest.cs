//using System;
//using System.Linq;
//using Fordere.RestService.Extensions;
//using Fordere.ServiceInterface.Messages.Match;
//using Fordere.ServiceInterface.Messages.User;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Fordere.UnitTest
//{
//    [TestClass]
//    public class MatchServiceTest
//    {
//        [ClassInitialize]
//        public static void ClassInitialize(TestContext context)
//        {
//            ServiceStarter.EnsureServiceIsRunning();
//        }           

//        [TestMethod]
//        public void AddMatch()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new AddMatchRequest
//                {
//                    BarId = "bars/1",
//                    GuestTeamId = "teams/1",
//                    HomeTeamId = "teams/2",
//                    PlayDate = new DateTime(2050, 1, 1, 20, 00, 0),
//                    RegisterDate = DateTime.Now
//                };

//            var newMatch = client.Post(request);

//            var match = client.Get(new GetMatchByIdRequest { Id = newMatch.Id.StripIdPrefix() });

//            Assert.IsNotNull(match, "Service didnt return expected match.");
//        }

//        [TestMethod]
//        public void EditMatch()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new UpdateMatchRequest
//            {
//                Id = "matches/1",
//                BarId = "bars/2",
//                GuestTeamId = "teams/2",
//                HomeTeamId = "teams/1"
//            };

//            var updatedMatch = client.Put(request);

//            Assert.IsNotNull(updatedMatch);

//            var match = client.Get(new GetMatchByIdRequest { Id = 1 });

//            Assert.IsNotNull(match, "Updated match should still be available after update.");
//            Assert.AreEqual("matches/1", match.Id);
//            Assert.AreEqual("teams/2", match.GuestTeam.Id);
//            Assert.AreEqual("teams/1", match.HomeTeam.Id);
//            Assert.AreEqual("bars/2", match.Bar.Id);
//        }

//        [TestMethod]
//        public void GetMatchById()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new GetUserByIdRequest
//                {
//                    Id = 1
//                };

//            var match = client.Get(request);

//            Assert.IsNotNull(match);
//            Assert.AreEqual(1, match.Id.StripIdPrefix());
//        }

//        [TestMethod]
//        public void GetAllMatches()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new GetAllMatchesRequest();

//            var response = client.Get(request);

//            var user1 = response.FirstOrDefault(p => p.Id == "matches/1");
//            var user2 = response.FirstOrDefault(p => p.Id == "matches/2");

//            Assert.IsNotNull(user1, "Seeded match not found with id 'matches/1'");
//            Assert.IsNotNull(user2, "Seeded match not found with id 'matches/2'");
//        }
//    }
//}
