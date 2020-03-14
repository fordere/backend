using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Team
{
    [Route("/teams/{Id}", "DELETE", Summary = "Delete a team.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DeleteTeamRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}