using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Bar
{
    [Route("/bars/{Id}", "GET", Summary = "Get a bar by Id.")]
    public class GetBarByIdRequest : IReturn<BarDto>
    {
        public int Id { get; set; }
    }
}