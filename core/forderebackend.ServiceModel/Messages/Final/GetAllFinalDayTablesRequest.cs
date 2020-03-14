using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/tables", "GET", Summary = "Get all finalday tables")]
    
    public class GetAllFinalDayTablesRequest : IReturn<List<FinalDayTableDto>>
    {
        public int FinalDayId { get; set; }
    }
}