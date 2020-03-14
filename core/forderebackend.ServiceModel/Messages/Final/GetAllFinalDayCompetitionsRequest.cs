using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competitions", "GET", Summary = "Gets all finalday competitions")]
    
    public class GetAllFinalDayCompetitionsRequest : IReturn<List<FinalDayCompetitionDto>>
    {
        public int FinalDayId { get; set; }
    }
}
