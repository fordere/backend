namespace Fordere.ServiceInterface.Dtos
{
    public class CompetitionPlayerStandingDto
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public UserDto Player { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesDraw { get; set; }

        public int GamesLost { get; set; }

        public int Points { get; set; }

        public int GoalsScored { get; set; }

        public int GoalsConceded { get; set; }

        public int Rank { get; set; }
    }
}