using System;
using System.Collections.Generic;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/tables/{TableId}/{Day}/timeslots", "GET", Summary = "Get timeslots for a specific day.")]
    
    public class TimeSlotRequest : IReturn<List<string>>
    {
        public int TableId { get; set; }
        public DateTime Day { get; set; }
    }

    // TODO Fix Return type
    [Route("/bars/timeslots", "GET", Summary = "Gets all bars with timeslots")]
    
    public class BarTimeSlotsRequest : IReturnVoid
    {
        
    }
}