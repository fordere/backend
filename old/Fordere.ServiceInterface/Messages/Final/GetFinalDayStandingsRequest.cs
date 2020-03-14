using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/season/current/finaldaystandings", "GET", Summary = "Gets all standings for the current seasons finalday")]
    [Route("/season/{SeasonId}/finaldaystandings", "GET", Summary = "Gets all standings for the current seasons finalday")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetFinalDayStandingsRequest
    {
        public int? SeasonId { get; set; }
    }
}