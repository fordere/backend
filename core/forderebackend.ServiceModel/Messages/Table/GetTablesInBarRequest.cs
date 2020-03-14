using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/bar/{BarId}/tables", "GET", Summary = "Gets all available tables in a bar")]
    
    public class GetTablesInBarRequest : IReturn<TableDto>
    {
        public int BarId { get; set; }
    }
}