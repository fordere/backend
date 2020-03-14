using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/{MatchId}/appointment", "DELETE")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DeleteMatchAppointmentRequest : IReturn<ExtendedMatchDto>
    {
        public int MatchId { get; set; }
    }
}