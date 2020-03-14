using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages
{
    [Route("/statistics/{SeasonId}", "GET", Summary = "Get all statistics")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllStatisticsRequest : IReturn<StatisticsDto>
    {
        public long SeasonId { get; set; }
    }
}