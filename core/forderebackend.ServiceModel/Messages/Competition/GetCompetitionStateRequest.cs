using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Competition
{
    [Route("/competitions/{CompetitionId}/state", "GET", Summary = "Returns the state of a competition")]
    
    public class GetCompetitionStateRequest : IReturn<CompetitionStateDto>
    {
        public int CompetitionId { get; set; }
    }


}