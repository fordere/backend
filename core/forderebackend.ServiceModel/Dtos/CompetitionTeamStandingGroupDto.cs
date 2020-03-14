using System.Collections.Generic;

namespace forderebackend.ServiceModel.Dtos
{
    public class CompetitionTeamStandingGroupDto
    {
        public CompetitionTeamStandingGroupDto()
        {
            Standings = new List<CompetitionTeamStandingViewDto>();
        }

        public int Number { get; set; }

        public int NumberOfSuccessor { get; set; }

        public List<CompetitionTeamStandingViewDto> Standings { get; set; }
    }
}