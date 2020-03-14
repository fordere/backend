using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos.FinalDay;
using forderebackend.ServiceModel.Messages.Season;

namespace forderebackend.ServiceModel.Dtos
{
    public class SeasonDto : NameDto
    {
        public List<CompetitionDto> Competitions { get; set; }

        public SeasonState State { get; set; }

        public FinalDayDto FinalDay { get; set; }

        public string Dates { get; set; }

        public string InfosFinalDay { get; set; }

        public string InfosPrepareSeason { get; set; }

        public string InfosEinteilung { get; set; }
    }
}