

using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/season/current/finaldaystandings", "GET", Summary = "Gets all standings for the current seasons finalday")]
    [Route("/season/{SeasonId}/finaldaystandings", "GET", Summary = "Gets all standings for the current seasons finalday")]
    
    public class GetFinalDayStandingsRequest
    {
        public int? SeasonId { get; set; }
    }
}