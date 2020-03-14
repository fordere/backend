using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities.Final
{
    public class Group
    {
        [AutoIncrement]
        public int Id { get; set; }

        public int Number { get; set; }

        [References(typeof(FinalDayCompetition))]
        public int FinalDayCompetitionId { get; set; }

        [Reference]
        public FinalDayCompetition FinalDayCompetition { get; set; }

        [Reference]
        public List<TeamInGroup> Teams { get; set; }

        public int NumberOfSuccessor { get; set; }

        [Reference]
        public List<CompetitionTeamStanding> CompetitionTeamStandings { get; set; } 
    }
}