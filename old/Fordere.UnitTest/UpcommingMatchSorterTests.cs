using System.Collections.Generic;

using Fordere.RestService.Entities;
using Fordere.RestService.FinalDay;

using NUnit.Framework;

namespace Fordere.UnitTest
{
    [TestFixture]
    public class UpcommingMatchSorterTests
    {
        [Test]
        public void NoSortingRequired()
        {
            var matches = new List<MatchView>
            {
                new MatchView {Id = 1, HomePlayer1Id = 1, HomePlayer2Id = 2, GuestPlayer1Id = 3, GuestPlayer2Id = 4},
                new MatchView {Id = 2, HomePlayer1Id = 5, HomePlayer2Id = 6, GuestPlayer1Id = 7, GuestPlayer2Id = 8},
                new MatchView {Id = 3, HomePlayer1Id = 9, HomePlayer2Id = 10, GuestPlayer1Id = 11, GuestPlayer2Id = 12}
            };

            var sorter = new UpcommingMatchSorter();
            var sortedMatches = sorter.Sort(null, matches, 1);

            Assert.AreEqual(3, sortedMatches.Count);
            Assert.AreEqual(1, sortedMatches[0].Id);
            Assert.AreEqual(2, sortedMatches[1].Id);
            Assert.AreEqual(3, sortedMatches[2].Id);
        }

        [Test]
        public void SimpleResort()
        {
            // P    | Count
            //  1   | 1
            //  2   | 1
            //  3   | 1
            //  4   | 1
            //  5   | 2
            //  6   | 1
            //  7   | 1
            //  8   | 1
            //  9   | 0
            // 10   | 1
            // 11   | 1
            // 12   | 1

            // M    | C                 | Rank
            // 1    | 1 + 1 + 1 + 1 = 4 > 3
            // 2    | 2 + 1 + 1 + 1 = 5 > 1
            // 3    | 2 + 1 + 1 + 1 = 5 > 2
            var matches = new List<MatchView>
            {
                new MatchView {Id = 1, HomePlayer1Id = 1, HomePlayer2Id = 2, GuestPlayer1Id = 3, GuestPlayer2Id = 4},
                new MatchView {Id = 2, HomePlayer1Id = 5, HomePlayer2Id = 6, GuestPlayer1Id = 7, GuestPlayer2Id = 8},
                new MatchView {Id = 3, HomePlayer1Id = 5, HomePlayer2Id = 10, GuestPlayer1Id = 11, GuestPlayer2Id = 12}
            };

            var sorter = new UpcommingMatchSorter();
            var sortedMatches = sorter.Sort(null, matches, 1);

            Assert.AreEqual(3, sortedMatches.Count);
            Assert.AreEqual(2, sortedMatches[0].Id);
            Assert.AreEqual(3, sortedMatches[1].Id);
            Assert.AreEqual(1, sortedMatches[2].Id);
        }

        [Test]
        public void ComplexResort()
        {
            // P  | Count
            //  1 | 1
            //  2 | 2
            //  3 | 1
            //  4 | 2
            //  5 | 4
            //  6 | 1
            //  7 | 1
            //  8 | 1
            //  9 | 0
            // 10 | 1
            // 11 | 3
            // 12 | 3

            // M  | C                   | Rank
            // 1  | 1 + 2 + 1 + 2 = 6   > 5
            // 2  | 4 + 1 + 2 + 1 = 8   > 4
            // 3  | 4 + 2 + 3 + 3 = 12  > 1
            // 4  | 4 + 1 + 3 + 3 = 11  > 3
            // 5  | 4 + 2 + 3 + 3 = 12  > 2
            var matches = new List<MatchView>
            {
                new MatchView {Id = 1, HomePlayer1Id = 1, HomePlayer2Id = 2, GuestPlayer1Id = 3, GuestPlayer2Id = 4},
                new MatchView {Id = 2, HomePlayer1Id = 5, HomePlayer2Id = 6, GuestPlayer1Id = 7, GuestPlayer2Id = 8},
                new MatchView {Id = 3, HomePlayer1Id = 5, HomePlayer2Id = 4, GuestPlayer1Id = 11, GuestPlayer2Id = 12},
                new MatchView {Id = 4, HomePlayer1Id = 5, HomePlayer2Id = 10, GuestPlayer1Id = 11, GuestPlayer2Id = 12},
                new MatchView {Id = 5, HomePlayer1Id = 5, HomePlayer2Id = 2, GuestPlayer1Id = 11, GuestPlayer2Id = 12}

            };

            var sorter = new UpcommingMatchSorter();
            var sortedMatches = sorter.Sort(null, matches, 1);

            Assert.AreEqual(5, sortedMatches.Count);
            Assert.AreEqual(3, sortedMatches[0].Id);
            Assert.AreEqual(5, sortedMatches[1].Id);
            Assert.AreEqual(4, sortedMatches[2].Id);
            Assert.AreEqual(2, sortedMatches[3].Id);
            Assert.AreEqual(1, sortedMatches[4].Id);
        }


    }
}
