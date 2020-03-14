using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions", "GET", Summary = "Gets all competitions")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllCompetitionsRequest : IReturn<List<CompetitionDto>>
    {
    }
}