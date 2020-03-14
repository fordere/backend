using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/tables", "GET", Summary = "Get all finalday tables")]
    
    public class GetAllFinalDayTablesRequest : IReturn<List<FinalDayTableDto>>
    {
        public int FinalDayId { get; set; }
    }
}