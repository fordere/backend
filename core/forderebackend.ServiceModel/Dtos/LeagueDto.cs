using System.Collections.Generic;

namespace forderebackend.ServiceModel.Dtos
{
    public class LeagueDto : NameDto
    {
        public string SeasonId { get; set; }
        public int Number { get; set; }
        public int Group { get; set; }
        public List<TableEntryViewDto> TableEntries { get; set; }

        public CompetitionDto Competition { get; set; }

        public LeagueMatchCreationMode LeagueMatchCreationMode { get; set; }

        public override string ToString()
        {
            return string.Format("{0} (Group {1}) [{2}]", this.Name, this.Group, this.Id);
        }
    }

    
    public class LeagueWithTeamsDto : NameDto
    {
        public int Number { get; set; }

        public int Group { get; set; }

        public List<TeamViewDto> Teams { get; set; }
    }
}