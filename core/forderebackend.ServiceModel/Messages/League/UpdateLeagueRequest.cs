using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.League
{
    [Route("/leagues/{Id}", "PUT", Summary = "Update league")]
    
    public class UpdateLeagueRequest : IReturn<LeagueDto>
    {
        public int Id { get; set; }
        public LeagueMatchCreationMode LeagueMatchCreationMode { get; set; }
        public int Number { get; set; }
        public int Group { get; set; }
    }
}