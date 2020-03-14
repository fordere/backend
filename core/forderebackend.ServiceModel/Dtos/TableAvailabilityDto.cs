using System;

namespace forderebackend.ServiceModel.Dtos
{
    public class TableAvailabilityDto : BaseDto
    {
        public int FirstTimeSlotDayOfWeek { get; set; }

        public int LastTimeSlotDayOfWeek { get; set; }

        public DateTime FirstTimeSlot { get; set; }

        public DateTime LastTimeSlot { get; set; }
    }
}