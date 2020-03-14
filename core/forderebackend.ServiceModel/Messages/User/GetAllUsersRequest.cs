using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/users", "GET", Summary = "Get users.")]
    public class GetAllUsersRequest : PagedRequest, IReturn<UsersResponse>
    {
        public string Filter { get; set; }
    }
}