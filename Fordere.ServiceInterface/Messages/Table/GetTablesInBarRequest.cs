using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/bar/{BarId}/tables", "GET", Summary = "Gets all available tables in a bar")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetTablesInBarRequest : IReturn<TableDto>
    {
        public int BarId { get; set; }
    }
}