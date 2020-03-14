using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Cup
{
    [Route("/cups/{Id}", "GET", Summary = "Get cup by Id.")]
    public class GetCupByIdRequest : IReturn<Cup2Dto>
    {
        public int Id { get; set; }
    }
}