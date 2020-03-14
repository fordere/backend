using System;
using forderebackend.ServiceModel.Messages.Table;

namespace forderebackend.ServiceModel.Dtos
{
    // TODO SSH: Why this DTO? Eigentlich sollte das MatchView Entity reichen...
    // allerdings.. :)
    public class MatchViewDto
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public int GuestTeamId { get; set; }
        public bool IsNotPlayedMatch { get; set; }
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
        public bool HomeTeamIsForfaitOut { get; set; }
        public bool GuestTeamIsForfaitOut { get; set; }
        public int HomePlayer1Id { get; set; }
        public int HomePlayer2Id { get; set; }
        public int GuestPlayer1Id { get; set; }
        public int GuestPlayer2Id { get; set; }
        public string BarName { get; set; }
        public string CupName { get; set; }
        public int? CompetitionId { get; set; }
        public int SeasonId { get; set; }
        public string CompetitionName { get; set; }
        public int LeagueNumber { get; set; }
        public bool IsFreeTicket { get; set; }
        public int LeagueGroup { get; set; }
        public int FinalDayCompetitionId { get; set; }
        public string FinalDayCompetitionName { get; set; }
        public int FinalDayTableNumber { get; set; }
        public TableType FinalDayTableType { get; set; }
        public int? FinalDayTableId { get; set; }
        public int? TableId { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? GuestTeamScore { get; set; }

        public bool IsHomeTeamFreeTicket => IsFreeTicket && HomeTeamScore < GuestTeamScore;
        public bool IsGuestTeamFreeTicket => IsFreeTicket && GuestTeamScore < HomeTeamScore;
    }

    public class MatchDto
    {
        public MatchDto()
        {
            HomeTeam = new TeamDto();
            GuestTeam = new TeamDto();
            Bar = new NameDto();
        }

        public string Id { get; set; }
        public TeamDto HomeTeam { get; set; }
        public TeamDto GuestTeam { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? GuestTeamScore { get; set; }
        public NameDto Bar { get; set; }
        public DateTime PlayDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime ResultDate { get; set; }
        public int? CupRound { get; set; }
    }
}