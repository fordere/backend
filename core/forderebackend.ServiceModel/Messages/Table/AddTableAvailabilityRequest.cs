using System;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/tables/{TableId}/availabilities", "PUT", Summary = "Get all tables available in a bar")]
    
    public class AddTableAvailabilityRequest : IReturn<TableAvailabilityDto>
    {
        public int TableId { get; set; }

        public int FirstTimeSlotDayOfWeek { get; set; }

        public DateTime FirstTimeSlot { get; set; }

        public int LastTimeSlotDayOfWeek { get; set; }

        public DateTime LastTimeSlot { get; set; }
    }
}