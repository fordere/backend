using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/users/me", "GET", Summary = "Gets the minimal user information of the logged in user.")]
    public class GetMyUserDetailsRequest : IReturn<UserDto>
    {
    }
}