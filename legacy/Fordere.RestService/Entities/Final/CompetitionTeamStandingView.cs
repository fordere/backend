using Fordere.ServiceInterface.Messages.Final;

namespace Fordere.RestService.Entities.Final
{
    public class CompetitionTeamStandingView
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

        public int GroupNumberOfSuccessor { get; set; }

        public int GroupNumber { get; set; }

        public int FinalDayCompetitionId { get; set; }

        public string FinalDayCompetitionName { get; set; }

        public CompetitionMode CompetitionMode { get; set; }

        public int FinalDayId { get; set; }
    }
}