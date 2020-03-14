using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Cup
{
    [Route("/cups/{Id}", "GET", Summary = "Get cup by Id.")]
    public class GetCupByIdRequest : IReturn<Cup2Dto>
    {
        public int Id { get; set; }
    }
}