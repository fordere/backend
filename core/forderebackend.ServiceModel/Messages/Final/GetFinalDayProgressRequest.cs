using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/{FinalDayId}/progress", "GET")]
    public class GetFinalDayProgressRequest : IReturn<List<FinalDayCompetitionProgressDto>>
    {
        public int FinalDayId { get; set; }
    }
}