using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competitions", "GET", Summary = "Gets all finalday competitions")]
    
    public class GetAllFinalDayCompetitionsRequest : IReturn<List<FinalDayCompetitionDto>>
    {
        public int FinalDayId { get; set; }
    }
}
