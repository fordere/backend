namespace forderebackend.ServiceModel.Dtos.FinalDay
{
    public class FinalDayPlayerInCompetitionDto
    {
        public int Id { get; set; }

        public UserDto Player { get; set; }

        public FinalDayCompetitionDto[] ActiveFinalDayCompetitions { get; set; }

        public bool IsActive { get; set; }
    }
}
