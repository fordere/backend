using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/recovery", "POST",
        Summary =
            "Create RecoveryToken for a user using Email. The user will receive an Email with a link to set a new password.")]
    public class ResetPasswordRequest : IReturnVoid
    {
        public string Email { get; set; }
    }
}