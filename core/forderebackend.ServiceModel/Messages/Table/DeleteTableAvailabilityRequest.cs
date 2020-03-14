

using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/tables/{TableId}/availabilities", "DELETE", Summary = "Get all tables available in a bar")]
    
    public class DeleteTableAvailabilityRequest : IReturnVoid
    {
        public int TableId { get; set; }

        public int TableAvailabilityId { get; set; }
    }
}