using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/leagues/{Id}/matches", "GET", Summary = "Get matches from league.")]
    
    public class GetMatchesByLeagueRequest : IReturn<ExtendedMatchDto>
    {
        [ApiMember(Name = "Id", Description = "Id of the league", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }
}