using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/{Id}", "GET", Summary = "Gets competition by Id")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetCompetitionByIdRequest : IReturn<CompetitionDto>
    {
        public int Id { get; set; }
    }
}