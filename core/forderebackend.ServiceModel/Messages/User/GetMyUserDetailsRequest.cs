using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/users/me", "GET", Summary = "Gets the minimal user information of the logged in user.")]
    public class GetMyUserDetailsRequest : IReturn<UserDto>
    {
    }
}