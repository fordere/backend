using System.Collections.Generic;

namespace forderebackend.ServiceModel.Dtos.FinalDay
{
    public class GroupDto
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int FinalDayCompetitionId { get; set; }

        public FinalDayCompetitionDto FinalyDayCompetition { get; set; }

        public List<TeamInGroupViewDto> Teams { get; set; }

        public int NumberOfSuccessor { get; set; }
    }
}