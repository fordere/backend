using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.League
{
    [Route("/leagues/{Id}", "GET", Summary = "Get league by Id")]
    
    public class GetLeagueByIdRequest : IReturn<LeagueDto>
    {
        public int Id { get; set; }
    }
}