using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/users/me/profile", "PUT", Summary = "Update user profile (Firstname, Lastname, Email, Password, PhoneNumber).")]
    [Route("/users/profile", "PUT", Summary = "Update user profile (Firstname, Lastname, E-Mail, Password, PhoneNumber).")]
    public class UpdateUserProfileRequest : IReturn<UserProfileResponse>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public bool DivisionZuerich { get; set; }
        public bool DivisionStGallen { get; set; }
        public bool DivisionLuzern { get; set; }
        public bool DivisionWinti { get; set; }
    }
}