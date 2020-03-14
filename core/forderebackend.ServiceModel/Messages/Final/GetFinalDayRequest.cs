
using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/{Id}", "GET", Summary = "Get a single finalday")]
    
    public class GetFinalDayRequest : IReturn<FinalDayDto>
    {
        public int Id { get; set; }
    }
}