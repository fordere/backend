
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.League
{
    [Route("/leagues/{Id}", "GET", Summary = "Get league by Id")]
    
    public class GetLeagueByIdRequest : IReturn<LeagueDto>
    {
        public int Id { get; set; }
    }
}