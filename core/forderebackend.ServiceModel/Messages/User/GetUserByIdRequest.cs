using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/users/{Id}", "GET", Summary = "Get user by Id.")]
    public class GetUserByIdRequest : IReturn<UserDto>
    {
        public int Id { get; set; }
    }
}