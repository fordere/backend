using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/users/{Id}", "GET", Summary = "Get user by Id.")]
    public class GetUserByIdRequest : IReturn<UserDto>
    {
        public int Id { get; set; }
    }
}