using System;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages;
using forderebackend.ServiceModel.Messages.Match;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Web;

namespace forderebackend.ServiceInterface.Filters
{
    public static class Filter
    {
        /// <summary>
        /// Ensures the authenticated user is either involved in the match or has the admin role.
        /// </summary>
        public static void EnterMatchAppointment(IRequest request, IResponse response, EnterMatchAppointmentRequest arg3)
        {
            // TODO: move the code from the services to here
        }

        /// <summary>
        /// Ensures the authenticated user is either involved in the match or has the admin role. If the ResultDate is
        /// older than 10 minutes, the Admin role is required to edit the match.
        /// </summary>
        public static void EnterMatchResult(IRequest request, IResponse response, EnterMatchResultRequest arg3)
        {
            // TODO: move the code from the services to here
        }

        /// <summary>
        /// Ensures the Captcha is filled in correctly.
        /// </summary>
        public static void Captcha(IRequest request, IResponse response, ICaptchaRequest dtoRequest)
        {
            if (!CaptchaSolver.Solve(request.RemoteIp, dtoRequest.Captcha))
            {
                throw new ArgumentException("Spamschutz wurde falsch ausgefüllt! Versuchs nocheinmal oder wende dich direkt per Mail an uns!");
            }
        }

        /// <summary>
        /// Remove all the user specific details from the teamdto when the user is not authenticated
        /// </summary>
        public static void TeamPlayerDetails(IRequest request, IResponse response, TeamDto team)
        {
            if (request.GetSession().HasRole(RoleNames.Admin, HostContext.Resolve<IUserAuthRepository>()))
            {
                return;
            }

            if (request.GetSession().IsAuthenticated)
            {
                using (var db = HostContext.Resolve<IDbConnectionFactory>().Open())
                {
                    var currentUserId = Convert.ToInt32(request.GetSession().UserAuthId);

                    var isUserPlayingInTheSameLeague = db.Select<Team>(sql => (sql.Player1Id == currentUserId || sql.Player2Id == currentUserId) && sql.LeagueId == team.League.Id).Any();

                    if (isUserPlayingInTheSameLeague)
                    {
                        return;
                    }
                }
            }

            team.Player1.Email = string.Empty;
            team.Player1.PhoneNumber = string.Empty;
            team.Player2.Email = string.Empty;
            team.Player2.PhoneNumber = string.Empty;
        }
    }
}
