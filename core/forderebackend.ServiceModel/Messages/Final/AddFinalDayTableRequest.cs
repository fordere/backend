using forderebackend.ServiceModel.Dtos.FinalDay;
using forderebackend.ServiceModel.Messages.Table;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/tables", "POST", Summary = "Add a FinalDay Table")]
    public class AddFinalDayTableRequest : IReturn<FinalDayTableDto>
    {
        public int FinalDayId { get; set; }

        public int Number { get; set; }

        public TableType TableType { get; set; }
    }
}