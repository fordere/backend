using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/bar/tables/{BarId}/competition/{CompetitionId}", "GET", Summary = "Get all tables available in a bar")]
    
    public class GetTablesInBarForCompetitionRequest : IReturn<List<TableDto>>
    {
        public int BarId { get; set; }

        public int CompetitionId { get; set; }
    }
}