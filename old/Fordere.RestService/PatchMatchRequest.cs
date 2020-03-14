using System;

using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.RestService
{
    [Route("/matches/{Id}", "PATCH", Summary = "Updates a single match")]
    public class PatchMatchRequest : IReturn<MatchDto>
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }

        public int GuestTeamId { get; set; }

        public int? FinalDayCompetitionId { get; set; }

        public int? TableId { get; set; }

        public DateTime? PlayDate { get; set; }

        public DateTime? RegisterDate { get; set; }

        public DateTime? ResultDate { get; set; }

        public int LeagueId { get; set; }

        public int CupId { get; set; }

        public int CupRound { get; set; }

        public bool IsWinnerBracketGame { get; set; }

        public int? RoundOrder { get; set; }

        public bool IsNotPlayedMatch { get; set; }
    }
}