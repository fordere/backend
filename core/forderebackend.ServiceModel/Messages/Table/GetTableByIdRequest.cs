using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/tables/{TableId}", "GET", Summary = "Gets a single table")]
    public class GetTableByIdRequest : IReturn<TableDto>
    {
        public int TableId { get; set; }
    }
}