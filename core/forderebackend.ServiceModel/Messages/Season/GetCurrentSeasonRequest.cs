using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Season
{
    [Route("/seasons/current", "GET", Summary = "Gets the current running season")]
    public class GetCurrentSeasonRequest : IReturn<SeasonDto>
    {
    }
}