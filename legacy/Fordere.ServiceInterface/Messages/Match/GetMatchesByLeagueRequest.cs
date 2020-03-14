using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/leagues/{Id}/matches", "GET", Summary = "Get matches from league.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetMatchesByLeagueRequest : IReturn<ExtendedMatchDto>
    {
        [ApiMember(Name = "Id", Description = "Id of the league", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }
}