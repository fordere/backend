using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/{FinalDayId}/progress", "GET")]
    public class GetFinalDayProgressRequest : IReturn<List<FinalDayCompetitionProgressDto>>
    {
        public int FinalDayId { get; set; }
    }
}