
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/tables", "GET", Summary = "Gets all available tables")]
    
    public class GetAllTablesRequest : IReturn<TableDto>
    {
    }
}