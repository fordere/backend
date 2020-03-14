

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Team
{
    [Route("/teams/{Id}", "DELETE", Summary = "Delete a team.")]
    
    public class DeleteTeamRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}