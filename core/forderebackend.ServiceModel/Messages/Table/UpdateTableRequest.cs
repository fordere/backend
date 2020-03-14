using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/tables/{Id}", "Put", Summary = "Create a single table")]
    public class UpdateTableRequest : IReturn<TableDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TableType TableType { get; set; }
    }
}