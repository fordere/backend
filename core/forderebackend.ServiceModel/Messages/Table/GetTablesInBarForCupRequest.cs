using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/bar/tables/{BarId}/cup/{CupId}", "GET", Summary = "Get all tables available in a bar")]
    
    public class GetTablesInBarForCupRequest : IReturn<List<TableDto>>
    {
        public int BarId { get; set; }

        public int CupId { get; set; }
    }
}