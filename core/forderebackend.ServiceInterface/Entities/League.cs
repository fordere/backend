using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class League : IFordereObject
    {
        [AutoIncrement] public int Id { get; set; }

        [References(typeof(Competition))] public int CompetitionId { get; set; }

        [Reference] public Competition Competition { get; set; }

        public int Number { get; set; }

        public int Group { get; set; }

        [Reference] public List<Team> Teams { get; set; }

        [Reference] public List<TableEntry> TableEntries { get; set; }

        public LeagueMatchCreationMode LeagueMatchCreationMode { get; set; }

        public string StandingsOrder { get; set; }
    }
}