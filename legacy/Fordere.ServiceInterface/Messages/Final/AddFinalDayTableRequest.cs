using Fordere.ServiceInterface.Dtos.FinalDay;
using Fordere.ServiceInterface.Messages.Table;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/tables", "POST", Summary = "Add a FinalDay Table")]
    public class AddFinalDayTableRequest : IReturn<FinalDayTableDto>
    {
        public int FinalDayId { get; set; }

        public int Number { get; set; }

        public TableType TableType { get; set; }
    }
}