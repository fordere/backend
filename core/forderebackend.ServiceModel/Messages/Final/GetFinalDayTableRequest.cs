using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/tables/{Id}", "GET", Summary = "Get a single finalday table")]
    public class GetFinalDayTableRequest : IReturn<FinalDayTableDto>
    {
        public int Id { get; set; }
    }
}