using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Season
{
    [Route("/seasons/{Id}", "GET", Summary = "Gets the current running season")]
    public class GetSeasonById : IReturn<SeasonDto>
    {
        public long Id { get; set; }
    }
}