//using System.Collections.Generic;
//using System.Linq;

//using Fordere.RestService.Entities;
//using Fordere.RestService.LeagueExecution;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Fordere.UnitTest
//{
//    [TestClass]
//    public class MatchFactoryTests
//    {
//        [TestMethod]
//        public void TestFreeTicketCount()
//        {

//        }

//        [TestMethod]
//        public void LeagueEveryTeamHasSameAmountOfMatches()
//        {
//            // Arrange
//            var teamInscriptions = this.CreateTeams(20);

//            // Act
//            IEnumerable<Match> matches = MatchFactory.CreateLeagueMatches(teamInscriptions);

//            // Assert
//            var matchCounter = new Dictionary<int, int>();
//            foreach (Match match in matches)
//            {
//                matchCounter.CountUp(match.HomeTeamId);
//                matchCounter.CountUp(match.GuestTeamId);
//            }

//            Assert.AreEqual(1, matchCounter.Values.Distinct().Count());
//            Assert.AreEqual(19, matchCounter.Values.First());
//        }

//        [TestMethod]
//        public void LeagueHomeTeamIsNeverSameAsGuestTeam()
//        {
//            // Arrange
//            var teamInscriptions = this.CreateTeams(20);

//            // Act
//            IEnumerable<Match> matches = MatchFactory.CreateLeagueMatches(teamInscriptions);

//            // Assert
//            Assert.IsFalse(matches.Any(x => x.HomeTeamId == x.GuestTeamId));
//        }


//        [TestMethod]
//        public void LeagueSameNumberOfHomeAndGuestGamesPerTeamWithEvenNumberOfTeams()
//        {
//            // Arrange
//            var teamInscriptions = this.CreateTeams(20);

//            // Act
//            IEnumerable<Match> matches = MatchFactory.CreateLeagueMatches(teamInscriptions);

//            // Assert
//            var guestMatches = new Dictionary<int, int>();
//            var homeMatches = new Dictionary<int, int>();
//            foreach (Match match in matches)
//            {
//                homeMatches.CountUp(match.HomeTeamId);
//                guestMatches.CountUp(match.GuestTeamId);
//            }

//            // When there is a even number of teams some teams will have 1 more home match then others -> 2 different values
//            Assert.IsTrue(homeMatches.Values.Distinct().Count() == 2);
//            Assert.IsTrue(guestMatches.Values.Distinct().Count() == 2);

//            Assert.IsTrue(homeMatches.Values.Distinct().Any(x => x == 9));
//            Assert.IsTrue(homeMatches.Values.Distinct().Any(x => x == 10));

//            Assert.IsTrue(guestMatches.Values.Distinct().Any(x => x == 9));
//            Assert.IsTrue(guestMatches.Values.Distinct().Any(x => x == 10));
//        }

//        [TestMethod]
//        public void LeagueSameNumberOfHomeAndGuestGamesPerTeamWithOddNumberOfTeams()
//        {
//            // Arrange
//            var teamInscriptions = this.CreateTeams(21);

//            // Act
//            IEnumerable<Match> matches = MatchFactory.CreateLeagueMatches(teamInscriptions);

//            // Assert
//            var guestMatches = new Dictionary<int, int>();
//            var homeMatches = new Dictionary<int, int>();
//            foreach (Match match in matches)
//            {
//                homeMatches.CountUp(match.HomeTeamId);
//                guestMatches.CountUp(match.GuestTeamId);
//            }

//            // Every team has the same amount of home and guest matches
//            Assert.IsTrue(homeMatches.Values.Distinct().Count() == 1);
//            Assert.IsTrue(guestMatches.Values.Distinct().Count() == 1);

//            // Number of home matches is equal to guest matches
//            Assert.AreEqual(homeMatches.Values.First(), guestMatches.Values.First());

//            // Number of matches (home/guest) have to be 10 -> 21 Teams -> 20 Matches -> 10 Home / 10 Guest
//            Assert.AreEqual(10, homeMatches.Values.First());
//        }

//        [TestMethod]
//        public void CupEvenNumberOfTeams()
//        {
//            // Arrange
//            var teams = this.CreateTeams(20);
//            var leagues = new[] { new League { Id = 1, CompetitionId = 1, Number = 1, Group = 0 } }.ToList();
//            teams.ForEach(t => t.LeagueId = 1);

//            // Act
//            List<Match> matches = MatchFactory.CreateCupMatches(teams, leagues, null);

//            // Assert
//            Assert.AreEqual(16, matches.Count);
//        }

//        [TestMethod]
//        public void CupOddNumberOfTeams()
//        {
//            // Arrange
//            var teamInscriptions = this.CreateTeams(19);
//            var leagues = new List<League> { new League { Id = 1, Number = 1 } };

//            // Act
//            List<Match> matches = MatchFactory.CreateCupMatches(teamInscriptions, leagues, null);

//            // Assert
//            Assert.AreEqual(16, matches.Count);
//        }

//        [TestMethod]
//        public void CupEveryTeamsDoesPlayJustOnce()
//        {
//            // Arrange
//            var teamInscriptions = this.CreateTeams(20);
//            var leagues = new List<League> { new League { Id = 1, Number = 1 } };

//            // Act
//            List<Match> matches = MatchFactory.CreateCupMatches(teamInscriptions, leagues, null);

//            // Assert
//            IEnumerable<int> plaayingTeamIds = matches.Select(f => new List<int>() { f.HomeTeamId, f.GuestTeamId }).SelectMany(item => item).Distinct();
//            Assert.AreEqual(20, plaayingTeamIds.Count());
//        }

//        [TestMethod]
//        public void CupUnderClassifiedTeamIsAlwaysHomeTeam()
//        {
//            // Arrange
//            var teams = new List<Team>
//                                   {
//                                       new Team {Id=1, LeagueId = 2 },
//                                       new Team {Id=2, LeagueId = 1 }
//                                   };

//            var leagues = new List<League>
//                          {
//                              new League { Id = 1, Number = 1 },
//                              new League { Id = 2, Number = 2 }
//                          };

//            // Act
//            List<Match> matches = MatchFactory.CreateCupMatches(teams, leagues, null);

//            // Assert
//            Assert.AreEqual(1, matches.Count);
//            Match match = matches.First();

//            Assert.AreEqual(1, match.HomeTeamId);
//            Assert.AreEqual(2, match.GuestTeamId);
//        }

//        private List<Team> CreateTeams(int count)
//        {
//            var list = new List<Team>();

//            for (int i = 0; i < count; i++)
//            {
//                list.Add(new Team { Id = i + 1 , LeagueId = 1 });
//            }

//            return list;
//        }
//    }

//    public static class DictionaryExtensions
//    {
//        public static void CountUp(this Dictionary<int, int> dict, int key)
//        {
//            if (dict.ContainsKey(key))
//            {
//                dict[key] = dict[key] + 1;
//            }
//            else
//            {
//                dict.Add(key, 1);
//            }
//        }
//    }
//}
