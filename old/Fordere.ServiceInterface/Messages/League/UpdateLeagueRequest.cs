using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.League
{
    [Route("/leagues/{Id}", "PUT", Summary = "Update league")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class UpdateLeagueRequest : IReturn<LeagueDto>
    {
        public int Id { get; set; }
        public LeagueMatchCreationMode LeagueMatchCreationMode { get; set; }
        public int Number { get; set; }
        public int Group { get; set; }
    }
}