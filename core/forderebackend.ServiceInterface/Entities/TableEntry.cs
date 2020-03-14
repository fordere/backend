using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class TableEntry : IFordereObject
    {
        public int Id { get; set; }

        [References(typeof(League))] public int LeagueId { get; set; }

        [References(typeof(Team))] public int TeamId { get; set; }

        [Reference] public Team Team { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesNotPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesWonExtension { get; set; }

        public int GamesDraw { get; set; }

        public int GamesLostExtension { get; set; }

        public int GamesLost { get; set; }

        public int Points { get; set; }

        public int SetsWon { get; set; }

        public int SetsLost { get; set; }

        public int Rank { get; set; }

        public int EstimatedRank { get; set; }

        [Ignore] public int PlusMinus => SetsWon - SetsLost;
    }
}