using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/matches/{MatchId}/appointment", "DELETE")]
    public class DeleteMatchAppointmentRequest : IReturn<ExtendedMatchDto>
    {
        public int MatchId { get; set; }
    }
}