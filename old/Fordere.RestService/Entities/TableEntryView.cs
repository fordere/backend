using Fordere.ServiceInterface.Dtos;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    public class TableEntryView : IFordereObject
    {
        public int Id { get; set; }

        public int LeagueId { get; set; }

        public int TeamId { get; set; }

        public bool IsTeamForfaitOut { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesNotPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesWonExtension { get; set; }

        public int GamesDraw { get; set; }

        public int GamesLost { get; set; }

        public int GamesLostExtension { get; set; }

        public int Points { get; set; }

        public int SetsWon { get; set; }

        public int SetsLost { get; set; }

        public int Rank { get; set; }

        public int EstimatedRank { get; set; }

        public string TeamName { get; set; }

        public string Player1FirstName { get; set; }

        public string Player1LastName { get; set; }

        public string Player2FirstName { get; set; }

        public string Player2LastName { get; set; }

        public QualifiedForFinalDay QualifiedForFinalDay { get; set; }

        public string QualifiedForFinalDayComment { get; set; }

        [Ignore]
        public int PlusMinus
        {
            get { return SetsWon - SetsLost; }
        }
    }
}