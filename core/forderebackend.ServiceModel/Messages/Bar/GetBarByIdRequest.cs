
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Bar
{
    [Route("/bars/{Id}", "GET", Summary = "Get a bar by Id.")]
    
    public class GetBarByIdRequest : IReturn<BarDto>
    {
        public int Id { get; set; }
    }
}