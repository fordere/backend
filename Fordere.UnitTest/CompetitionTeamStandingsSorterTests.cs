using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Fordere.RestService.Entities;
using Fordere.RestService.Entities.Final;
using Fordere.RestService.FinalDay;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Match = Fordere.RestService.Entities.Match;

namespace Fordere.UnitTest
{
    [TestClass]
    public class CompetitionTeamStandingsSorterTests
    {
        [TestMethod]
        public void TestWinCircle()
        {
            // Team 1: Gaht di nüt a    Butterface      1628
            // Team 2: Hubertus         12              1722
            // Team 3: Brothers         Random          1634
            // Team 4: Chunky           Alegam          1701

            var standings = new List<CompetitionTeamStanding>
            {
                new CompetitionTeamStanding{Points = 9, GamesWon = 3, GamesLost = 1, GoalsScored = 6, GoalsConceded = 0, Id = 1, TeamId = 1},
                new CompetitionTeamStanding{Points = 3, GamesWon = 1, GamesLost = 2, GoalsScored = 3, GoalsConceded = 4, Id = 2, TeamId = 2},
                new CompetitionTeamStanding{Points = 3, GamesWon = 1, GamesLost = 2, GoalsScored = 2, GoalsConceded = 4, Id = 3, TeamId = 3},
                new CompetitionTeamStanding{Points = 3, GamesWon = 1, GamesLost = 2, GoalsScored = 2, GoalsConceded = 5, Id = 4, TeamId = 4},
            };


            var matches = new List<Match>
            {
                new Match{ HomeTeamId = 1, GuestTeamId = 3, HomeTeamScore = 2, GuestTeamScore = 0}, // Gaht di vs. Brothers     2:0 1628 vs. 1634
                new Match{ HomeTeamId = 1, GuestTeamId = 4, HomeTeamScore = 2, GuestTeamScore = 0}, // Gaht di vs. Chunky       2:0 1628 vs. 1701
                new Match{ HomeTeamId = 3, GuestTeamId = 4, HomeTeamScore = 2, GuestTeamScore = 0}, // Brothers vs. Chunky      2:0 1634 vs. 1701
                new Match{ HomeTeamId = 3, GuestTeamId = 2, HomeTeamScore = 0, GuestTeamScore = 2}, // Brothers vs. Hubertus    0:2 1634 vs. 1722
                new Match{ HomeTeamId = 4, GuestTeamId = 2, HomeTeamScore = 2, GuestTeamScore = 1}, // Chunky vs. Hubertus      2:1 1701 vs. 1722
                new Match{ HomeTeamId = 2, GuestTeamId = 1, HomeTeamScore = 0, GuestTeamScore = 2}, // Hubertus vs. Gaht di     0:2 1722 vs. 1628 
              
               
                };

            var sorted = CompetitionTeamStandingsSorter.OrderByRanking(standings, matches).ToList();

            Assert.AreEqual(1, sorted.Skip(0).Take(1).Single().Id); // Gaht di nüt a
            Assert.AreEqual(2, sorted.Skip(1).Take(1).Single().Id); // Hubertus
            Assert.AreEqual(3, sorted.Skip(2).Take(1).Single().Id); // Brothers
            Assert.AreEqual(4, sorted.Skip(3).Take(1).Single().Id); // Chunky Monkey
        }

        [TestMethod]
        public void TestWinCircle2()
        {
            // Team 1: Miley
            // Team 2: Velvet
            // Team 3: Butter
            // Team 4: Rand

            var standings = new List<CompetitionTeamStanding>
            {
                new CompetitionTeamStanding{Points = 9, GamesWon = 3, GamesLost = 0, GoalsScored = 5, GoalsConceded = 2, Id = 1, TeamId = 1},
                new CompetitionTeamStanding{Points = 3, GamesWon = 2, GamesLost = 1, GoalsScored = 4, GoalsConceded = 4, Id = 2, TeamId = 2},
                new CompetitionTeamStanding{Points = 3, GamesWon = 2, GamesLost = 1, GoalsScored = 2, GoalsConceded = 5, Id = 3, TeamId = 3},
                new CompetitionTeamStanding{Points = 3, GamesWon = 2, GamesLost = 1, GoalsScored = 4, GoalsConceded = 5, Id = 4, TeamId = 4},
            };


            var matches = new List<Match>
            {
                new Match{ HomeTeamId = 4, GuestTeamId = 3, HomeTeamScore = 1, GuestTeamScore = 2},
                new Match{ HomeTeamId = 4, GuestTeamId = 1, HomeTeamScore = 1, GuestTeamScore = 2},
                new Match{ HomeTeamId = 4, GuestTeamId = 2, HomeTeamScore = 2, GuestTeamScore = 1},
                new Match{ HomeTeamId = 3, GuestTeamId = 1, HomeTeamScore = 0, GuestTeamScore = 2},
                new Match{ HomeTeamId = 3, GuestTeamId = 2, HomeTeamScore = 0, GuestTeamScore = 2},
                new Match{ HomeTeamId = 1, GuestTeamId = 2, HomeTeamScore = 2, GuestTeamScore = 1}, 
              
               
                };

            var sorted = CompetitionTeamStandingsSorter.OrderByRanking(standings, matches).ToList();

            Assert.AreEqual(1, sorted.Skip(0).Take(1).Single().Id); // Miley
            Assert.AreEqual(2, sorted.Skip(1).Take(1).Single().Id); // Velvet
            Assert.AreEqual(3, sorted.Skip(2).Take(1).Single().Id); // Butter
            Assert.AreEqual(4, sorted.Skip(3).Take(1).Single().Id); // Rand
        }
    }
}
