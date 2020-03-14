using System;
using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/tables/{TableId}/{Day}/timeslots", "GET", Summary = "Get timeslots for a specific day.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TimeSlotRequest : IReturn<List<string>>
    {
        public int TableId { get; set; }
        public DateTime Day { get; set; }
    }

    // TODO Fix Return type
    [Route("/bars/timeslots", "GET", Summary = "Gets all bars with timeslots")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BarTimeSlotsRequest : IReturnVoid
    {
        
    }
}