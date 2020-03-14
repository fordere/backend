using System.Collections.Generic;

using Fordere.RestService.Entities;

namespace Fordere.RestService.LeagueExecution
{
    public interface IMatchCreator
    {
        List<Match> CreateMatches(IList<Team> teams);

        List<Match> CreateMatches(IList<Team> existingTeams, Team newTeam);
    }
}