using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Table
{
    [Route("/tables/{Id}", "Put", Summary = "Create a single table")]
    public class UpdateTableRequest : IReturn<TableDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TableType TableType { get; set; }
    }
}