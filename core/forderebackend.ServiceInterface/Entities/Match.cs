using System;
using forderebackend.ServiceInterface.Entities.Final;
using forderebackend.ServiceInterface.LeagueExecution.Standings;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class Match : IFordereObject
    {
        private int? homeTeamScore;
        private int? guestTeamScore;

        [AutoIncrement] public int Id { get; set; }

        [Reference] public Team HomeTeam { get; set; }

        [Reference] public Team GuestTeam { get; set; }

        [References(typeof(Team))] public int HomeTeamId { get; set; }

        [References(typeof(Team))] public int GuestTeamId { get; set; }

        [References(typeof(FinalDayCompetition))]
        public int? FinalDayCompetitionId { get; set; }

        [Reference] public FinalDayCompetition FinalDayCompetition { get; set; }

        public int? HomeTeamScore
        {
            get
            {
                if (HomeTeamId == default(int)) return 0;

                if (homeTeamScore.HasValue == false && GuestTeamId == default(int)) return 1;

                if (HomeTeam != null && GuestTeam != null)
                {
                    if (HomeTeam.IsForfaitOut) return StandingsCalculator.SetsLossForfait;

                    if (GuestTeam.IsForfaitOut && !HomeTeam.IsForfaitOut) return StandingsCalculator.SetsWinForfait;
                }

                return homeTeamScore;
            }
            set => homeTeamScore = value;
        }

        public int? GuestTeamScore
        {
            get
            {
                if (GuestTeamId == default(int)) return 0;

                if (guestTeamScore.HasValue == false && HomeTeamId == default(int)) return 1;

                if (GuestTeam != null && HomeTeam != null)
                {
                    if (GuestTeam.IsForfaitOut) return StandingsCalculator.SetsLossForfait;

                    if (HomeTeam.IsForfaitOut && !GuestTeam.IsForfaitOut) return StandingsCalculator.SetsWinForfait;
                }

                return guestTeamScore;
            }
            set => guestTeamScore = value;
        }

        [References(typeof(Table))] public int? TableId { get; set; }

        [Reference] public Table Table { get; set; }

        public int? FinalDayTableId { get; set; }

        [Reference] public FinalDayTable FinalDayTable { get; set; }

        public DateTime? PlayDate { get; set; }

        public DateTime? RegisterDate { get; set; }

        public DateTime? ResultDate { get; set; }

        public int LeagueId { get; set; }

        public int CupId { get; set; }

        public int CupRound { get; set; }

        public bool IsWinnerBracketGame { get; set; }

        public int? RoundOrder { get; set; }

        public bool IsNotPlayedMatch { get; set; }

        [Ignore] public bool HasResult => HomeTeamScore.HasValue && GuestTeamScore.HasValue;

        [Ignore]
        public bool IsDraw
        {
            get
            {
                if (HasResult) return HomeTeamScore.GetValueOrDefault() == GuestTeamScore.GetValueOrDefault();

                return false;
            }
        }

        [Ignore]
        public bool IsExtensionPlayed
        {
            get
            {
                if (HasResult)
                    // TODO Number of win sets should somehow be configurable
                    return HomeTeamScore.GetValueOrDefault() + GuestTeamScore.GetValueOrDefault() == 5;

                return false;
            }
        }

        [Ignore]
        public int WinnerTeamId
        {
            get
            {
                if (HasResult == false) return 0;

                if (HomeTeamScore.GetValueOrDefault() > GuestTeamScore.GetValueOrDefault()) return HomeTeamId;

                if (HomeTeamScore.GetValueOrDefault() < GuestTeamScore.GetValueOrDefault()) return GuestTeamId;

                return 0;
            }
        }


        [Ignore]
        public int LoserTeamId
        {
            get
            {
                if (HasResult == false) return 0;

                if (WinnerTeamId == HomeTeamId) return GuestTeamId;

                return HomeTeamId;
            }
        }

        [Ignore]
        public bool Overtime
        {
            get
            {
                if (HasResult == false) return false;

                if (HomeTeamScore.GetValueOrDefault() == StandingsCalculator.SetsWinOvertime &&
                    GuestTeamScore.GetValueOrDefault() == StandingsCalculator.SetsLossOvertime ||
                    GuestTeamScore.GetValueOrDefault() == StandingsCalculator.SetsWinOvertime &&
                    HomeTeamScore.GetValueOrDefault() == StandingsCalculator.SetsLossOvertime)
                    return true;

                return false;
            }
        }

        public override string ToString()
        {
            return HomeTeamId + " vs. " + GuestTeamId;
        }
    }
}