using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competitions/{Id}/state", "POST", Summary = "Update the state of a FinalDay Competition")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class UpdateFinalDayCompetitionStateRequest : IReturnVoid
    {
        public int Id { get; set; }

        public FinalDayCompetitionState CompetitionState { get; set; }
    }
}