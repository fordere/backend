using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competitions/{Id}", "GET", Summary = "Get's a single finalday competition")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetFinalDayCompetitionRequest : IReturn<FinalDayCompetitionDto>
    {
        public int FinalDayId { get; set; }

        public int Id { get; set; }
    }
}
