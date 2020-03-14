using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/competitions/{Id}/matches", "GET", Summary = "Get matches from competition.")]
    
    public class GetMatchesByCompetitionRequest : IReturn<ExtendedMatchDto>
    {
        [ApiMember(Name = "Id", Description = "Id of the competition", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }
}