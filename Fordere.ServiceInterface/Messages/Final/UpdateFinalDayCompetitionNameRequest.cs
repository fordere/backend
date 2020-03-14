using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competitions/{Id}/name", "POST", Summary = "Update the state of a FinalDay Competition")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class UpdateFinalDayCompetitionNameRequest : IReturnVoid
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}