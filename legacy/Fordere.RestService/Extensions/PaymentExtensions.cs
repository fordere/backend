using Fordere.RestService.Entities;
using Fordere.ServiceInterface.Dtos;

namespace Fordere.RestService.Extensions
{
    public static class PaymentExtensions
    {
        public static PaymentDto ToDto(this Payment payment)
        {
            return new PaymentDto
                   {
                       Id = payment.Id,
                       UserId = payment.UserId,
                       HasPaid = payment.HasPaid,
                       Comment = payment.Comment,
                       User = payment.User.ToDto()
                   };
        }
    }
}