using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/bar/tables/{BarId}/competition/{CompetitionId}", "GET", Summary = "Get all tables available in a bar")]
    public class GetTablesInBarForCompetitionRequest : IReturn<List<TableDto>>
    {
        public int BarId { get; set; }

        public int CompetitionId { get; set; }
    }
}