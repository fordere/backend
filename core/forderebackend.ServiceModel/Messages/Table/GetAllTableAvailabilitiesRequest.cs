using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/tables/{TableId}/availabilities", "GET", Summary = "Get all tables available in a bar")]
    public class GetAllTableAvailabilitiesRequest : IReturn<TableAvailabilityDto>
    {
        public int TableId { get; set; }
    }
}