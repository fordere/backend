using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Bar
{
    [Route("/divisions", "GET", Summary = "Gets all Divisions")]
    public class GetAllDivisionsRequest : IReturn<DivisionDto>
    {
    }
}