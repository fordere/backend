
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/competitions/{Id}/matches", "GET", Summary = "Get matches from competition.")]
    
    public class GetMatchesByCompetitionRequest : IReturn<ExtendedMatchDto>
    {
        [ApiMember(Name = "Id", Description = "Id of the competition", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }
}