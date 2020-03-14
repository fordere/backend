using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages
{
    [Route("/statistics/{SeasonId}", "GET", Summary = "Get all statistics")]
    
    public class GetAllStatisticsRequest : IReturn<StatisticsDto>
    {
        public long SeasonId { get; set; }
    }
}