using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Bar
{
    [Route("/divisions/payment", "GET")]
    public class GetPaymentInformationsRequest : IReturn<DivisionDto>
    {
        
    }
}