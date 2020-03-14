using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/users/me/profile", "GET", Summary = "Gets the user profile of the logged in user.")]
    public class GetMyUserProfileRequest : IReturn<UserProfileResponse>
    {
    }
}