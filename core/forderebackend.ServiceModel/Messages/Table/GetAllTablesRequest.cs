using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/tables", "GET", Summary = "Gets all available tables")]
    public class GetAllTablesRequest : IReturn<TableDto>
    {
    }
}