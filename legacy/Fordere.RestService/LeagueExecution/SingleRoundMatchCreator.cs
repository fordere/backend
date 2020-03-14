using System.Collections.Generic;
using System.Linq;

using Fordere.RestService.Entities;
using Fordere.RestService.Extensions;

namespace Fordere.RestService.LeagueExecution
{
    public class SingleRoundMatchCreator : IMatchCreator
    {
        public List<Match> CreateMatches(IList<Team> teams)
        {
            var listOfMatches = new List<Match>();
            int halfNumberOfTeams = teams.Count / 2;

            for (int i = 0; i < teams.Count; i++)
            {
                for (int j = 1; j <= halfNumberOfTeams; j++)
                {
                    var firstTeam = teams[i];
                    int opponentIndex = (i + j) % teams.Count;
                    var secondTeam = teams[opponentIndex];

                    if (!DoesMatchAlreadyExist(listOfMatches, firstTeam, secondTeam))
                    {
                        var match = new Match { HomeTeamId = firstTeam.Id, GuestTeamId = secondTeam.Id, PlayDate = null };
                        listOfMatches.Add(match);
                    }
                }
            }

            return listOfMatches;
        }

        public List<Match> CreateMatches(IList<Team> existingTeamsInLeague, Team movedTeam)
        {
            var homeGameTickets = new List<bool>(existingTeamsInLeague.Count);

            FillListWithHomeGameTicketsRandomed(existingTeamsInLeague, homeGameTickets);

            var newMatches = new List<Match>(existingTeamsInLeague.Count);

            foreach (var team in existingTeamsInLeague)
            {
                if (team.Id != movedTeam.Id)
                {
                    newMatches.Add(CreateMatch(homeGameTickets.Pop(0), movedTeam, team));
                }
            }

            return newMatches;
        }

        private static bool DoesMatchAlreadyExist(IEnumerable<Match> listOfMatches, Team firstTeam, Team secondTeam)
        {
            return listOfMatches.Any(x => (x.HomeTeamId == firstTeam.Id && x.GuestTeamId == secondTeam.Id) || x.HomeTeamId == secondTeam.Id && x.GuestTeamId == firstTeam.Id);
        }

        private static Match CreateMatch(bool firstTeamIsAtHome, Team firstTeam, Team secondTeam)
        {
            var match = new Match();

            if (firstTeamIsAtHome)
            {
                match.HomeTeamId = firstTeam.Id;
                match.GuestTeamId = secondTeam.Id;
            }
            else
            {
                match.HomeTeamId = secondTeam.Id;
                match.GuestTeamId = firstTeam.Id;
            }

            return match;
        }

        private static void FillListWithHomeGameTicketsRandomed(IList<Team> existingTeamsInLeague, IList<bool> list)
        {
            for (var i = 0; i < existingTeamsInLeague.Count / 2; ++i)
            {
                list.Add(true);
            }

            for (var i = existingTeamsInLeague.Count / 2; i < existingTeamsInLeague.Count; ++i)
            {
                list.Add(false);
            }

            list.Shuffle();
        }

    }
}
