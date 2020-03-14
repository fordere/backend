﻿
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/tables/{TableId}/availabilities", "GET", Summary = "Get all tables available in a bar")]
    
    public class GetAllTableAvailabilitiesRequest : IReturn<TableAvailabilityDto>
    {
        public int TableId { get; set; }
    }
}