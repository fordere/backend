using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Skills
{
    [Route("/skills/calculate", "POST", Summary = "Calculates all skills for all users")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CalculateSkillsRequest : IReturnVoid
    {
    }
}
