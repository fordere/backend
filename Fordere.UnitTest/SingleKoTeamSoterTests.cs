using System.Collections.Generic;

using Fordere.RestService.Entities.Final;
using Fordere.RestService.FinalDay;

using NUnit.Framework;

namespace Fordere.UnitTest
{
    [TestFixture]
    public class SingleKoTeamSoterTests
    {
        [Test]
        public void Sort()
        {
            var input = new List<TeamInGroup>
            {
                new TeamInGroup {Settlement = 1},
                new TeamInGroup {Settlement = 2},
                new TeamInGroup {Settlement = 3},
                new TeamInGroup {Settlement = 4},
                new TeamInGroup {Settlement = 5},
                new TeamInGroup {Settlement = 6},
                new TeamInGroup {Settlement = 7},
                new TeamInGroup {Settlement = 8},
                new TeamInGroup {Settlement = 9},
                new TeamInGroup {Settlement = 10},
                new TeamInGroup {Settlement = 11},
                new TeamInGroup {Settlement = 12},
                new TeamInGroup {Settlement = 13},
                new TeamInGroup {Settlement = 14},
                new TeamInGroup {Settlement = 15},
                new TeamInGroup {Settlement = 16},
            };

            var output = SingleKoTeamSorter.Sort(input);

            Assert.AreEqual(16, output.Count);
            Assert.AreEqual(1, output[0].Settlement);
            Assert.AreEqual(16, output[1].Settlement);
            Assert.AreEqual(9, output[2].Settlement);
            Assert.AreEqual(8, output[3].Settlement);
            Assert.AreEqual(5, output[4].Settlement);
            Assert.AreEqual(12, output[5].Settlement);
            Assert.AreEqual(13, output[6].Settlement);
            Assert.AreEqual(4, output[7].Settlement);
            Assert.AreEqual(2, output[8].Settlement);
            Assert.AreEqual(15, output[9].Settlement);
            Assert.AreEqual(10, output[10].Settlement);
            Assert.AreEqual(7, output[11].Settlement);
            Assert.AreEqual(6, output[12].Settlement);
            Assert.AreEqual(11, output[13].Settlement);
            Assert.AreEqual(14, output[14].Settlement);
            Assert.AreEqual(3, output[15].Settlement);
        }
    }
}
