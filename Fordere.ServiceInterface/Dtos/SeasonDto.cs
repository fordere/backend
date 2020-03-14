using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos.FinalDay;
using Fordere.ServiceInterface.Messages.Season;

namespace Fordere.ServiceInterface.Dtos
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