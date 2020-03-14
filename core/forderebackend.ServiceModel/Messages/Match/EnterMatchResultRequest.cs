
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/{Id}/result", "PUT")]
    
    public class EnterMatchResultRequest : IReturn<ExtendedMatchDto>
    {
        [ApiMember(Name = "Id", Description = "Id of the match", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }

        [ApiMember(Name = "HomeTeamScore", Description = "Score of the Home Team", ParameterType = "model", DataType = "int", IsRequired = true)]
        public int? HomeTeamScore { get; set; }

        [ApiMember(Name = "GuestTeamScore", Description = "Score of the Guest Team", ParameterType = "model", DataType = "int", IsRequired = true)]
        public int? GuestTeamScore { get; set; }
    }
}