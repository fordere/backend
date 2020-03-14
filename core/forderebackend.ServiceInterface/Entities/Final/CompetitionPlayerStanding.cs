using ServiceStack.Auth;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities.Final
{
    public class CompetitionPlayerStanding
    {
        [AutoIncrement] public int Id { get; set; }

        public int FinalDayCompetitionId { get; set; }

        [References(typeof(UserAuth))] public int PlayerId { get; set; }

        [Reference] public UserAuth Player { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesDraw { get; set; }

        public int GamesLost { get; set; }

        public int Points { get; set; }

        public int GoalsScored { get; set; }

        public int GoalsConceded { get; set; }

        public int Rank { get; set; }

        [Ignore] public int PlusMinus => GoalsScored - GoalsConceded;
    }
}