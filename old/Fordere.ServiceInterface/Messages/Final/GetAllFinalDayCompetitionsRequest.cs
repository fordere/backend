using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competitions", "GET", Summary = "Gets all finalday competitions")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllFinalDayCompetitionsRequest : IReturn<List<FinalDayCompetitionDto>>
    {
        public int FinalDayId { get; set; }
    }
}
