using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Fordere.ServiceInterface.Messages;
using Fordere.ServiceInterface.Messages.User;

using Newtonsoft.Json.Linq;

using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    /// <summary>
    /// Last ServiceStack updated failed to  mark the Member as const -> can't be used on Attributes :/.
    /// Until fixed we provide our own class with a const member to be used in Attributes. const > static :P
    /// </summary>
    public static class RoleNames
    {
        public const string Admin = "Admin";
    }

    public abstract class BaseService : Service
    {
        protected int? DivisionId
        {
            get
            {
                var divisionIdRaw = base.Request.GetHeader("division_id");

                int divisionId;

                if (int.TryParse(divisionIdRaw, out divisionId))
                {
                    return divisionId;
                }

                return null;
            }
        }

        protected TPagedResponse CreatePagedResponse<TPagedResponse>(PagedRequest request, int total) where TPagedResponse : PagedResponse, new()
        {
            var response = new TPagedResponse
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total
            };

            return response;
        }

        protected int SessionUserId
        {
            get { return Convert.ToInt32(this.GetSession().UserAuthId); }
        }

        public override void Dispose()
        {
            this.Db.Dispose();

            base.Dispose();
        }

        protected bool IsAdmin
        {
            get
            {
                var session = this.GetSession();

                return session.Roles != null && session.Roles.Contains(RoleNames.Admin);
            }
        }

        protected void EnsureIsAdmin()
        {
            if (this.IsAuthenticated == false)
            {
                throw new AuthenticationException();
            }

            if (this.IsAdmin == false)
            {
                throw new UnauthorizedAccessException();
            }
        }

        protected UserAuth GetAuthenticatedUser()
        {
            return this.Db.SingleById<UserAuth>(this.SessionUserId);
        }

        /// <summary>
        /// Patches the target from the JSON body. Don't forget to add the PreRequestFilters in AppHostConsole otherwise body will always be string.Empty
        /// </summary>
        protected List<string> Patch<T>(T target)
        {
            var body = Request.GetRawBody();

            var jsonObject = JObject.Parse(body);

            var fields = jsonObject.Children().Select(s => s.Path).ToList();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var propertyInfo = properties.First(p => p.Name == field);

                var jsonEntry = jsonObject[field];

                // todo: use changetype() for enums
                var value = ((JValue)jsonEntry).Value;

                if (propertyInfo.PropertyType.IsNullableType() && propertyInfo.PropertyType.GenericTypeArguments[0] == typeof(DateTime))
                {
                    propertyInfo.SetValue(target, value);
                }
                else
                {
                    var t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                    if (value != null)
                    {
                        propertyInfo.SetValue(target, Convert.ChangeType(value, t));
                    }
                    else
                    {
                        propertyInfo.SetValue(target, null);
                    }
                }
            }

            return fields;
        }

        protected object CachedForNonAdmins<T>(Func<T> func, string cacheKey, TimeSpan cacheTime)
        {
            if (IsAdmin)
            {
                return func();
            }

            return Request.ToOptimizedResultUsingCache(LocalCache, cacheKey, cacheTime, func);
        }
    }
}