using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
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