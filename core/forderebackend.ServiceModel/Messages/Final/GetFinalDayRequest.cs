using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/{Id}", "GET", Summary = "Get a single finalday")]
    
    public class GetFinalDayRequest : IReturn<FinalDayDto>
    {
        public int Id { get; set; }
    }
}