using System.Collections.Generic;
using forderebackend.ServiceInterface.Entities;

namespace forderebackend.ServiceInterface.LeagueExecution
{
    public interface IMatchCreator
    {
        List<Match> CreateMatches(IList<Team> teams);

        List<Match> CreateMatches(IList<Team> existingTeams, Team newTeam);
    }
}