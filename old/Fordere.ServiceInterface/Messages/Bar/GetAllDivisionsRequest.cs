using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Bar
{
    [Route("/divisions", "GET", Summary = "Gets all Divisions")]
    public class GetAllDivisionsRequest : IReturn<DivisionDto>
    {

    }
}