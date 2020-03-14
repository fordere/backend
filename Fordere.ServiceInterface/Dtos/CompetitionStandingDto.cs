using System.Collections.Generic;

using Fordere.ServiceInterface.Messages.Final;

namespace Fordere.ServiceInterface.Dtos
{
    public class CompetitionStandingDto
    {
        public CompetitionStandingDto()
        {
            Groups = new List<CompetitionTeamStandingGroupDto>();
            Players = new List<CompetitionPlayerStandingDto>();
        }

        public int CompetitionId { get; set; }

        public string CompetitionName { get; set; }

        public FinalDayCompetitionState CompetitionState { get; set; }

        public CompetitionMode CompetitionMode { get; set; }

        public List<CompetitionTeamStandingGroupDto> Groups { get; set; }

        public List<CompetitionPlayerStandingDto> Players { get; set; }

        public List<ExtendedMatchDto> SingleKoMatches { get; set; }
    }
}
