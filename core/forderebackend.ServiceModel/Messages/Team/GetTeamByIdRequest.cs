using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Team
{
    [Route("/teams/{Id}", "GET", Summary = "Get team by Id.")]
    public class GetTeamByIdRequest : IReturn<TeamDto>
    {
        public int Id { get; set; }
    }
}