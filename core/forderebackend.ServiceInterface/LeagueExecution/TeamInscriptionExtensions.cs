using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;

namespace forderebackend.ServiceInterface.LeagueExecution
{
    public static class TeamInscriptionExtensions
    {
        public static List<Team> CreateTeams(this IEnumerable<TeamInscription> teamInscriptions)
        {
            var inscriptions = teamInscriptions as IList<TeamInscription> ?? teamInscriptions.ToList();
            var teams = new List<Team>(inscriptions.Count());

            foreach (var inscription in inscriptions)
            {
                teams.Add(new Team
                {
                    BarId = inscription.BarId,
                    LeagueId = inscription.AssignedLeagueId,
                    Player1Id = inscription.Player1Id,
                    Player2Id = inscription.Player2Id,
                    Name = inscription.Name,
                    SeasonAmbition = inscription.SeasonAmbition,
                    WishPlayDay = inscription.WishPlayDay
                });
            }

            return teams;
        }
    }
}
