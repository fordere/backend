

namespace forderebackend.ServiceModel.Dtos
{
    
    public class TeamInscriptionDto : NameDto
    {
        public int CompetitionId { get; set; }

        public int Player1Id { get; set; }

        public UserDto Player1 { get; set; }

        public int Player2Id { get; set; }

        public UserDto Player2 { get; set; }

        public int? WishLeague { get; set; }

        public string SeasonAmbition { get; set; }

        public int BarId { get; set; }

        public string BarName { get; set; }

        public int? AssignedLeagueId { get; set; }
    }
}