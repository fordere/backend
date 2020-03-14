using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/bar/tables/{BarId}/cup/{CupId}", "GET", Summary = "Get all tables available in a bar")]
    
    public class GetTablesInBarForCupRequest : IReturn<List<TableDto>>
    {
        public int BarId { get; set; }

        public int CupId { get; set; }
    }
}