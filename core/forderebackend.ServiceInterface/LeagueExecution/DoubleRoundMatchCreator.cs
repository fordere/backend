using System.Collections.Generic;
using forderebackend.ServiceInterface.Entities;

namespace forderebackend.ServiceInterface.LeagueExecution
{
    public class DoubleRoundMatchCreator : IMatchCreator
    {
        public List<Match> CreateMatches(IList<Team> teams)
        {
            var matches = new List<Match>();
            foreach (var team in teams)
            foreach (var opponent in teams)
                if (team.Id != opponent.Id)
                {
                    matches.Add(new Match {HomeTeamId = team.Id, GuestTeamId = opponent.Id, PlayDate = null});
                }

            return matches;
        }

        public List<Match> CreateMatches(IList<Team> existingTeamsInLeague, Team movedTeam)
        {
            var matches = new List<Match>();
            foreach (var existingTeam in existingTeamsInLeague)
            {
                matches.Add(new Match {HomeTeamId = existingTeam.Id, GuestTeamId = movedTeam.Id, PlayDate = null});
                matches.Add(new Match {HomeTeamId = movedTeam.Id, GuestTeamId = existingTeam.Id, PlayDate = null});
            }

            return matches;
        }
    }
}