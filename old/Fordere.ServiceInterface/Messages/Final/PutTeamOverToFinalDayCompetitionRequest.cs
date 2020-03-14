using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competition/{Id}/putteamover", "POST", Summary = "Puts teams depending on given parameters into a group/finaldaycompetition")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PutTeamOverToFinalDayCompetitionRequest : IReturnVoid
    {
        public int Id { get; set; }

        public int? LeagueId { get; set; }

        public int? FinalDayCompetitionId { get; set; }

        public int? TeamId { get; set; }

        public int? GroupId { get; set; }

        public int? WalkoverInGroupId { get; set; }
    }
}