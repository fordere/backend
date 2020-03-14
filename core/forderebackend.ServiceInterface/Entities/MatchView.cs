using System;

using Fordere.RestService.LeagueExecution.Standings;
using Fordere.RestService.Properties;
using Fordere.ServiceInterface.Messages.Table;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    
    public class MatchView
    {
        private int? homeTeamScore;
        private int? guestTeamScore;
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public int GuestTeamId { get; set; }
        public bool IsNotPlayedMatch { get; set; }
        public int? BarId { get; set; }
        public int FinalDayCompetitionPriority { get; set; }
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
        public string HomeTeamWishPlayDay { get; set; }
        public string GuestTeamWishPlayDay { get; set; }
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
        public int? FinalDayCompetitionId { get; set; }
        public string FinalDayCompetitionName { get; set; }
        public int? FinalDayTableNumber { get; set; }
        public TableType FinalDayTableType { get; set; }
        public int? FinalDayTableId { get; set; }
        public int? TableId { get; set; }

        public int? HomeTeamScore
        {
            get
            {
                // TODO: This logic is duplicted in match.. de ganz Forfait shit is chli müäsam implementiert.. 
                if (HomeTeamIsForfaitOut)
                {
                    return StandingsCalculator.SetsLossForfait;
                }

                if (GuestTeamIsForfaitOut && !HomeTeamIsForfaitOut)
                {
                    return StandingsCalculator.SetsWinForfait;
                }

                return homeTeamScore;
            }
            set
            {
                homeTeamScore = value;
            }
        }

        public int? GuestTeamScore
        {
            get
            {
                if (GuestTeamIsForfaitOut)
                {
                    return StandingsCalculator.SetsLossForfait;
                }

                if (HomeTeamIsForfaitOut && !GuestTeamIsForfaitOut)
                {
                    return StandingsCalculator.SetsWinForfait;
                }

                return guestTeamScore;
            }
            set
            {
                guestTeamScore = value;
            }
        }

        [Ignore]
        public bool IsDraw
        {
            get
            {
                if (HasResult)
                {
                    return HomeTeamScore.GetValueOrDefault() == GuestTeamScore.GetValueOrDefault();
                }

                return false;
            }
        }

        [Ignore]
        public bool HasResult
        {
            get { return HomeTeamScore.HasValue && GuestTeamScore.HasValue; }
        }

        [Ignore]
        public int WinnerTeamId
        {
            get
            {
                if (HasResult == false)
                {
                    return 0;
                }

                if (HomeTeamScore.GetValueOrDefault() > GuestTeamScore.GetValueOrDefault())
                {
                    return HomeTeamId;
                }

                if (HomeTeamScore.GetValueOrDefault() < GuestTeamScore.GetValueOrDefault())
                {
                    return GuestTeamId;
                }

                return 0;
            }
        }

        [Ignore]
        public int WinnerTeamPlayer1
        {
            get
            {
                if (WinnerTeamId == HomeTeamId)
                {
                    return HomePlayer1Id;
                }

                return GuestPlayer1Id;
            }
        }

        [Ignore]
        public int WinnerTeamPlayer2
        {
            get
            {
                if (WinnerTeamId == HomeTeamId)
                {
                    return HomePlayer2Id;
                }

                return GuestPlayer2Id;
            }
        }

    }
}