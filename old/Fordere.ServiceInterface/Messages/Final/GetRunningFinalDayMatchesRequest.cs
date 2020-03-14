using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/{FinalDayId}/matches/running", "GET")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetRunningFinalDayMatchesRequest : IReturn<MatchDto>
    {
        public int FinalDayId { get; set; }
    }
}