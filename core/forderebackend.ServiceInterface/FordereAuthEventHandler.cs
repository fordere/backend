using System.Collections.Generic;
using forderebackend.ServiceInterface.Smtp;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Web;

namespace forderebackend.ServiceInterface
{
    
    public class FordereAuthEventHandler : AuthEvents
    {
        #region Overrides of AuthEvents

        public override void OnRegistered(IRequest httpReq, IAuthSession session, IServiceBase registrationService)
        {
            base.OnRegistered(httpReq, session, registrationService);

            var registerDto = (Register)httpReq.Dto;

            MailSender.SendWelcomeMail(registerDto.Email, registerDto.FirstName, registerDto.LastName);
        }

        #region Overrides of AuthEvents

        public override void OnAuthenticated(IRequest httpReq, IAuthSession session, IServiceBase authService, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            base.OnAuthenticated(httpReq, session, authService, tokens, authInfo);

            if (session.Roles == null)
            {
                session.Roles = new List<string>();
            }
        }

        #endregion

        #endregion
    }
}