using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities.Final
{
    public class CompetitionTeamStanding
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Group))]
        public int GroupId { get; set; }

        [Reference]
        public Group Group { get; set; }

        [References(typeof(Team))]
        public int TeamId { get; set; }

        [Reference]
        public Team Team { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesDraw { get; set; }

        public int GamesLost { get; set; }

        public int Points { get; set; }

        public int GoalsScored { get; set; }

        public int GoalsConceded { get; set; }

        public int Rank { get; set; }

        [Ignore]
        public int PlusMinus => GoalsScored - GoalsConceded;
    }
}