using System;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class Challenge : IFordereObject
    {
        public int Id { get; set; }

        public DateTime ProposedDate { get; set; }

        public DateTime AcceptedDate { get; set; }

        [References(typeof(Table))]
        public int TableId { get; set; }

        [Reference]
        public Table Table { get; set; }

        [References(typeof(Team))]
        public int ChallengingTeamId { get; set; }

        [References(typeof(Team))]
        public int AcceptingTeamId { get; set; }

        [Reference]
        public Team ChallengingTeam { get; set; }

        [Reference]
        public Team AcceptingTeam { get; set; }

        [Reference]
        public League League { get; set; }

        [References(typeof(League))]
        public int LeagueId { get; set; }
    }
}