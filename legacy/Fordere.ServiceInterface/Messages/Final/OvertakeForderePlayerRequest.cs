using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/overtakeforderplayer", "POST", Summary = "Adds fordere player to Competition")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class OvertakeForderePlayerRequest : IReturn<FinalDayPlayerInCompetitionDto>
    {
        public int FinalDayCompetitionId { get; set; }

        public int PlayerId { get; set; }
    }
}