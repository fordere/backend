using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/tables/{Id}", "POST", Summary = "Create a single table")]
    public class CreateTableRequest : IReturn<TableDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BarId { get; set; }
        public TableType TableType { get; set; }
    }
}