namespace Fordere.ServiceInterface.Dtos
{
    public class CompetitionTeamStandingViewDto
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int TeamId { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesDraw { get; set; }

        public int GamesLost { get; set; }

        public int Points { get; set; }

        public int GoalsScored { get; set; }

        public int GoalsConceded { get; set; }

        public int Rank { get; set; }

        public string TeamName { get; set; }

        public int FinalDayCompetitionId { get; set; }

        public int FinalDayId { get; set; }
    }
}