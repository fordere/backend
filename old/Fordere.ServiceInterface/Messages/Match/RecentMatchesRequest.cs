using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/recent", "GET", Summary = "Recent played matches (7 days).")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RecentMatchesRequest : IReturn<List<ExtendedMatchDto>>
    {
    }
}