
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Team
{
    [Route("/teams/{Id}", "GET", Summary = "Get team by Id.")]
    
    public class GetTeamByIdRequest : IReturn<TeamDto>
    {
        public int Id { get; set; }
    }
}