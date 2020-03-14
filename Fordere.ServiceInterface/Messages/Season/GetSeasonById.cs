using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Season
{
    [Route("/seasons/{Id}", "GET", Summary = "Gets the current running season")]
    public class GetSeasonById : IReturn<SeasonDto>
    {
        public long Id { get; set; }
    }
}