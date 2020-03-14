
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/{FinalDayId}/matches/running", "GET")]
    
    public class GetRunningFinalDayMatchesRequest : IReturn<MatchDto>
    {
        public int FinalDayId { get; set; }
    }
}