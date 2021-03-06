﻿using System;
using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Extensions;
using forderebackend.ServiceInterface.Smtp;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.User;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Logging;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class UserService : BaseService
    {
        private readonly IUserAuthRepository userAuthRepository;

        public UserService(IUserAuthRepository userAuthRepository)
        {
            this.userAuthRepository = userAuthRepository;
        }

        #region HTTP Verbs

        [Authenticate]
        public object Get(GetUserProfileRequest request)
        {
            var userAuth = Db.Select<UserAuth>(q => q.Id == request.Id).FirstOrDefault();

            userAuth.Throw404NotFoundIfNull("User not found");

            return userAuth.ToUserProfileResponse();
        }

        [Authenticate]
        public object Get(FindPossiblePartnersRequest request)
        {
            var alreadyRegisteredids =
                Db.Select(Db.From<TeamInscription>().Where(x => x.CompetitionId == request.CompetitionId))
                    .SelectMany(x => new List<int> {x.Player1Id, x.Player2Id}).ToList();

            var query = Db.From<UserAuth>()
                .Where(x =>
                    x.Id != SessionUserId &&
                    !Sql.In(x.Id, alreadyRegisteredids));

            foreach (var queryPart in request.Query.Split(' '))
                query = query.Where(x => x.FirstName.Contains(queryPart) || x.LastName.Contains(queryPart));

            query = query.OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Limit(8);

            return Db.Select(query).Select(x => x.ConvertTo<UserDto>()).ToList();
        }

        [Authenticate]
        public object Get(GetAllPossiblePartnersRequest request)
        {
            var teamInscriptions = Db.Select<TeamInscription>(s => s.CompetitionId == request.CompetitionId);

            var users =
                Db.Select<UserAuth>()
                    .Where(x => x.Id != SessionUserId && x.FirstName != null && x.LastName != null &&
                                !teamInscriptions.Any(t => t.Player1Id == x.Id || t.Player2Id == x.Id))
                    .Select(x => x.ToDto())
                    .ToList();

            //var users =
            //    Db.Select<UserAuth>()
            //        .Where(x => x.Id != SessionUserId &&  Db.Select<TeamInscription>().All(k => k.CompetitionId != request.CompetitionId || (k.Player1Id != x.Id && k.Player2Id != x.Id)))
            //        .Select(x => x.ToDto())
            //        .ToList();
            var namedUsers = users.Where(x => !string.IsNullOrEmpty(x.FirstName) && !string.IsNullOrEmpty(x.LastName));

            return new UsersResponse {Users = namedUsers.ToList()};
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetAllUsersRequest request)
        {
            var usersQuery = Db.From<UserAuth>().Where(p =>
                    p.FirstName != null && p.LastName != null && p.FirstName != "" && p.LastName != "")
                .OrderBy(o => o.FirstName)
                .ThenBy(o => o.LastName);

            request.SetLimitIfNoPagingRequested(20);

            var filter = request.Filter ?? "";
            usersQuery = usersQuery
                .Where(p => p.FirstName.Contains(filter) || p.LastName.Contains(filter) || p.Email.Contains(filter))
                .Limit(request.Offset, request.PageSize);

            var userAuths = Db.Select(usersQuery);
            var count = Db.Count(usersQuery);
            var result = userAuths.Select(s => s.ToDto()).ToList();
            var response = CreatePagedResponse<UsersResponse>(request, Convert.ToInt32(count));
            response.Users = result;

            return response;
        }

        [Authenticate]
        public object Put(UpdateUserProfileRequest request)
        {
            if (request.Id == default(int))
            {
                request.Id = SessionUserId;
            }

            if (SessionUserId != request.Id)
                // Only admins may change other users profile
            {
                EnsureIsAdmin();
            }

            var userAuth = Db.SingleById<UserAuth>(request.Id);
            var userAuthUpdated = Db.SingleById<UserAuth>(request.Id).PopulateWith(request);

            userAuthUpdated.SetDivision(UserAuthMetaKeys.DivisionLuzern, request.DivisionLuzern);
            userAuthUpdated.SetDivision(UserAuthMetaKeys.DivisionStGallen, request.DivisionStGallen);
            userAuthUpdated.SetDivision(UserAuthMetaKeys.DivisionZürich, request.DivisionZuerich);
            userAuthUpdated.SetDivision(UserAuthMetaKeys.DivisionWinti, request.DivisionWinti);

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                userAuthRepository.UpdateUserAuth(userAuth, userAuthUpdated);
            }
            else
            {
                userAuthRepository.UpdateUserAuth(userAuth, userAuthUpdated, request.Password);
            }


            return Db.SingleById<UserAuth>(request.Id).ToUserProfileResponse();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Put(UpdateUserRequest request)
        {
            var userAuth = Db.SingleById<UserAuth>(request.Id);
            var user = Db.SingleById<UserAuth>(request.Id);

            LogRoleChanges(request, user);

            user.PopulateWith(request);

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                userAuthRepository.UpdateUserAuth(userAuth, user);
            }
            else
            {
                userAuthRepository.UpdateUserAuth(userAuth, user, request.Password);
            }
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(CreateUserRequest request)
        {
            var userAuth = request.ConvertTo<UserAuth>();
            var createdUser = userAuthRepository.CreateUserAuth(userAuth, request.Password ?? CreateRandomPassword());

            return Get(new GetUserByIdRequest {Id = createdUser.Id});
        }

        private string CreateRandomPassword()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void Post(ResetPasswordRequest request)
        {
            var token = Guid.NewGuid().ToString("D");

            var userAuth = userAuthRepository.GetUserAuthByUserName(request.Email) as UserAuth;

            userAuth.Throw404NotFoundIfNull("Diese E-Mail ist uns unbekannt! Hast du vielleicht noch eine andere?");

            userAuth.RecoveryToken = token;

            Db.Update<UserAuth>(new {RecoveryToken = token}, p => p.Id == userAuth.Id);

            MailSender.SendPasswordRecoveryMail(userAuth);
        }

        public void Post(SetNewPasswordRequest request)
        {
            var userFromToken = Db.Single<UserAuth>(q => q.RecoveryToken == request.Token);

            userFromToken.Throw404NotFoundIfNull("Token not found.");

            var userAuth = (UserAuth) userAuthRepository.GetUserAuthByUserName(userFromToken.Email);
            var userAuthUpdate = (UserAuth) userAuthRepository.GetUserAuthByUserName(userFromToken.Email);
            userAuthUpdate.RecoveryToken = null;

            userAuthRepository.UpdateUserAuth(userAuth, userAuthUpdate, request.Password);
        }

        [Authenticate]
        public object Get(GetUserByIdRequest request)
        {
            var userAuth = Db.SingleById<UserAuth>(request.Id);

            var userAuthDto = userAuth.ToDto();

            return userAuthDto;
        }

        [Authenticate]
        public object Get(GetMyUserDetailsRequest request)
        {
            return Get(new GetUserByIdRequest {Id = SessionUserId});
        }

        [Authenticate]
        public object Get(GetMyUserProfileRequest request)
        {
            return Get(new GetUserProfileRequest {Id = SessionUserId});
        }

        #endregion

        private void LogRoleChanges(UpdateUserRequest request, UserAuth user)
        {
            var sessionUser = GetAuthenticatedUser();

            if (user.Roles != null && user.Roles.Contains(RoleNames.Admin) == false && request.Roles != null &&
                request.Roles.Contains(RoleNames.Admin))
            {
                LogManager.GetLogger(GetType())
                    .Info("{0} adds admin role to {1}".Fmt(sessionUser.ToDisplay(), user.ToDisplay()));
            }

            if (user.Roles != null && user.Roles.Contains(RoleNames.Admin) && request.Roles != null &&
                request.Roles.Contains(RoleNames.Admin) == false)
            {
                LogManager.GetLogger(GetType())
                    .Info("{0} removes admin role from {1}".Fmt(sessionUser.ToDisplay(), user.ToDisplay()));
            }
        }
    }
}