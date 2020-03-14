using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Season
{
    [Route("/seasons/current", "GET", Summary = "Gets the current running season")]
    public class GetCurrentSeasonRequest : IReturn<SeasonDto>
    {
    }
}