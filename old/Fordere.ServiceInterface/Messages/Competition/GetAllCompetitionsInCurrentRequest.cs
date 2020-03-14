using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/season/current", "GET", Summary = "Gets all competitions in current season")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllCompetitionsInCurrentRequest : IReturn<List<CompetitionDto>>
    {
    }
}