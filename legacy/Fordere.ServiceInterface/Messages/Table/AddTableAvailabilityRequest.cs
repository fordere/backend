using System;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/tables/{TableId}/availabilities", "PUT", Summary = "Get all tables available in a bar")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AddTableAvailabilityRequest : IReturn<TableAvailabilityDto>
    {
        public int TableId { get; set; }

        public int FirstTimeSlotDayOfWeek { get; set; }

        public DateTime FirstTimeSlot { get; set; }

        public int LastTimeSlotDayOfWeek { get; set; }

        public DateTime LastTimeSlot { get; set; }
    }
}