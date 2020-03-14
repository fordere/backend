using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using Fordere.RestService.Extensions;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Web;

namespace Fordere.WebConsole
{

    public class FordereAuthUserService : AuthUserSession
    {

        public override void OnRegistered(IRequest httpReq, IAuthSession session, IServiceBase registrationService)
        {
            var divisionId = GetDivisionId(httpReq);
            if (divisionId.HasValue)
            {
                var db = HostContext.AppHost.GetDbConnection(httpReq);
                var userAuth = db.SingleById<UserAuth>(session.UserAuthId);

                string key;
                switch (divisionId)
                {
                    case 1:
                        key = UserAuthMetaKeys.DivisionZürich;
                        break;
                    case 2:
                        key = UserAuthMetaKeys.DivisionStGallen;
                        break;
                    case 3:
                        key = UserAuthMetaKeys.DivisionLuzern;
                        break;
                    case 4:
                        key = UserAuthMetaKeys.DivisionWinti;
                        break;
                    default:
                        throw new Exception("Unknown division!");
                }

                userAuth.SetDivision(key, true);
                db.Update<UserAuth>(userAuth);
            }

            base.OnRegistered(httpReq, session, registrationService);

        }

        protected int? GetDivisionId(IRequest httpRequest)
        {
            var divisionIdRaw = httpRequest.GetHeader("division_id");

            int divisionId;

            if (int.TryParse(divisionIdRaw, out divisionId))
            {
                return divisionId;
            }

            return null;
        }

        public void OnAuthenticated(IRequest httpReq, IAuthSession session, IServiceBase authService, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
        }

        public void OnLogout(IRequest httpReq, IAuthSession session, IServiceBase authService)
        {
        }

        public void OnCreated(IRequest httpReq, IAuthSession session) { }
    }

    public class OrmLiteAuthRepository : OrmLiteAuthRepository<UserAuth, UserAuthDetails>, IUserAuthRepository
    {
        public OrmLiteAuthRepository(IDbConnectionFactory dbFactory) : base(dbFactory)
        {
        }
    }

}