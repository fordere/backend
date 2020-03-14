using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceModel.Dtos;

namespace forderebackend.ServiceInterface.Extensions
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