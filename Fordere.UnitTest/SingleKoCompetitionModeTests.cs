using System.Collections.Generic;

using Fordere.RestService.Entities.Final;
using Fordere.RestService.FinalDay;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fordere.UnitTest
{
    [TestClass]
    public class SingleKoCompetitionModeTests
    {
        [TestMethod]
        public void FullBracketWithEightOrderedTeams()
        {
            var teams = new List<TeamInGroup>
            {
                new TeamInGroup {TeamId = 1, Settlement = 1},
                new TeamInGroup {TeamId = 2, Settlement = 2},
                new TeamInGroup {TeamId = 3, Settlement = 3},
                new TeamInGroup {TeamId = 4, Settlement = 4},
                new TeamInGroup {TeamId = 5, Settlement = 5},
                new TeamInGroup {TeamId = 6, Settlement = 6},
                new TeamInGroup {TeamId = 7, Settlement = 7},
                new TeamInGroup {TeamId = 8, Settlement = 8},
            };

            var mode = new SingleKoCompetitionMode(null);
            var matches = mode.GenerateMatches(0);

            Assert.AreEqual(4, matches.Count);
            Assert.AreEqual(1, matches[0].HomeTeamId);
            Assert.AreEqual(8, matches[0].GuestTeamId);
            Assert.AreEqual(2, matches[1].HomeTeamId);
            Assert.AreEqual(7, matches[1].GuestTeamId);
            Assert.AreEqual(3, matches[2].HomeTeamId);
            Assert.AreEqual(6, matches[2].GuestTeamId);
            Assert.AreEqual(4, matches[3].HomeTeamId);
            Assert.AreEqual(5, matches[3].GuestTeamId);
        }

        [TestMethod]
        public void FullBracketWithEightNotOrdererTeams()
        {
            var teams = new List<TeamInGroup>
            {
                new TeamInGroup {TeamId = 5, Settlement = 5},
                new TeamInGroup {TeamId = 2, Settlement = 2},
                new TeamInGroup {TeamId = 6, Settlement = 6},
                new TeamInGroup {TeamId = 7, Settlement = 7},
                new TeamInGroup {TeamId = 8, Settlement = 8},
                new TeamInGroup {TeamId = 3, Settlement = 3},
                new TeamInGroup {TeamId = 4, Settlement = 4},
                new TeamInGroup {TeamId = 1, Settlement = 1},
            };

            var mode = new SingleKoCompetitionMode(null);
            var matches = mode.GenerateMatches(0);

            Assert.AreEqual(4, matches.Count);
            Assert.AreEqual(1, matches[0].HomeTeamId);
            Assert.AreEqual(8, matches[0].GuestTeamId);
            Assert.AreEqual(2, matches[1].HomeTeamId);
            Assert.AreEqual(7, matches[1].GuestTeamId);
            Assert.AreEqual(3, matches[2].HomeTeamId);
            Assert.AreEqual(6, matches[2].GuestTeamId);
            Assert.AreEqual(4, matches[3].HomeTeamId);
            Assert.AreEqual(5, matches[3].GuestTeamId);
        }

        [TestMethod]
        public void BracketFromSevenTeams()
        {
            var teams = new List<TeamInGroup>
            {
                new TeamInGroup {TeamId = 1, Settlement = 1},
                new TeamInGroup {TeamId = 2, Settlement = 2},
                new TeamInGroup {TeamId = 3, Settlement = 3},
                new TeamInGroup {TeamId = 4, Settlement = 4},
                new TeamInGroup {TeamId = 5, Settlement = 5},
                new TeamInGroup {TeamId = 6, Settlement = 6},
                new TeamInGroup {TeamId = 7, Settlement = 7},
            };

            var mode = new SingleKoCompetitionMode(null);
            var matches = mode.GenerateMatches(0);

            Assert.AreEqual(4, matches.Count);
            Assert.AreEqual(1, matches[0].HomeTeamId);
            Assert.AreEqual(-1, matches[0].GuestTeamId);
            Assert.AreEqual(2, matches[1].HomeTeamId);
            Assert.AreEqual(7, matches[1].GuestTeamId);
            Assert.AreEqual(3, matches[2].HomeTeamId);
            Assert.AreEqual(6, matches[2].GuestTeamId);
            Assert.AreEqual(4, matches[3].HomeTeamId);
            Assert.AreEqual(5, matches[3].GuestTeamId);
        }
    }
}
