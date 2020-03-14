using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Bar
{
    [Route("/divisions/payment", "GET")]
    public class GetPaymentInformationsRequest : IReturn<DivisionDto>
    {
    }
}