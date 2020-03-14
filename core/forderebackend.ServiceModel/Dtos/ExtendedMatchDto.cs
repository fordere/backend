using System;



namespace Fordere.ServiceInterface.Dtos
{
    
    public class ExtendedMatchDto
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public int GuestTeamId { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? GuestTeamScore { get; set; }
        public int? BarId { get; set; }
        public DateTime? PlayDate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ResultDate { get; set; }
        public int? LeagueId { get; set; }
        public int? CupId { get; set; }
        public int? CupRound { get; set; }
        public bool IsWinnerBracketGame { get; set; }
        public int RoundOrder { get; set; }
        public string HomeTeamName { get; set; }
        public string GuestTeamName { get; set; }
        public int HomePlayer1Id { get; set; }
        public int HomePlayer2Id { get; set; }
        public int GuestPlayer1Id { get; set; }
        public int GuestPlayer2Id { get; set; }
        public string BarName { get; set; }
        public string CupName { get; set; }
        public int? CompetitionId { get; set; }
        public int SeasonId { get; set; }
        public bool IsFreeTicket { get; set; }
    }
}