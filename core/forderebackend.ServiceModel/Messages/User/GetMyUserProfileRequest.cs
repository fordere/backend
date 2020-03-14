using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/users/me/profile", "GET", Summary = "Gets the user profile of the logged in user.")]
    public class GetMyUserProfileRequest : IReturn<UserProfileResponse>
    {
    }
}