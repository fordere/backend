using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/recovery/password", "POST", Summary = "Set a new password using a Recovery Token.")]
    public class SetNewPasswordRequest : IReturnVoid
    {
        public string Token { get; set; }

        public string Password { get; set; }
    }
}