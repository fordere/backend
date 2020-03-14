using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.League
{
    [Route("/leagues/{Id}/teams", "POST", Summary = "Move a team to a specific league.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class MoveTeamToLeagueRequest : IReturnVoid
    {
        public int Id { get; set; }

        public int TeamId { get; set; }
    }
}