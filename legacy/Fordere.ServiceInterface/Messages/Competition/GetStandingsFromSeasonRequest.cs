using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/seasons/{SeasonId}/standings", "GET", Summary = "Get all ranking tables of a season.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetStandingsFromSeasonRequest : IReturn<List<CompetitionDto>>
    {
        public int SeasonId { get; set; }
    }
}