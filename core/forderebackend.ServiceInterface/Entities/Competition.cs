using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class Competition : IFordereObjectWithName
    {
        [AutoIncrement] public int Id { get; set; }

        public string Name { get; set; }

        [References(typeof(Season))] public int SeasonId { get; set; }

        [Reference] public Season Season { get; set; }

        [Reference] public List<League> Leagues { get; set; }

        [Reference] public Cup Cup { get; set; }

        [Reference] public List<TableTypeInCompetition> TableTypes { get; set; }

        [Reference] public List<TeamInscription> TeamInscriptions { get; set; }

        [CustomField("MEDIUMTEXT")] public string RegistrationText { get; set; }

        public string Rules { get; set; }

        public string Modus { get; set; }
    }
}