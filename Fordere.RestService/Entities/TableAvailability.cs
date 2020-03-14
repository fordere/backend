using System;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    public class TableAvailability : IFordereObject
    {
        [AutoIncrement]
        public int Id { get; set; }

        public int TableId { get; set; }

        public int FirstTimeSlotDayOfWeek { get; set; }

        public DateTime FirstTimeSlot { get; set; }

        public int LastTimeSlotDayOfWeek { get; set; }

        public DateTime LastTimeSlot { get; set; }
    }
}