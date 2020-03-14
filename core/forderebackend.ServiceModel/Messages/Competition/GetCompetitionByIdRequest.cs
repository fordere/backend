using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Competition
{
    [Route("/competitions/{Id}", "GET", Summary = "Gets competition by Id")]
    
    public class GetCompetitionByIdRequest : IReturn<CompetitionDto>
    {
        public int Id { get; set; }
    }
}