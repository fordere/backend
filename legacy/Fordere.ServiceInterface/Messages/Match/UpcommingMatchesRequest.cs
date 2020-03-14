using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/upcomming", "GET", Summary = "Upcomming matches for the next 7 days.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class UpcommingMatchesRequest : IReturn<List<ExtendedMatchDto>>
    {
    }
}