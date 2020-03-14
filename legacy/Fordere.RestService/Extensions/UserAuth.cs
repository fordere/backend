using System;
using System.Collections.Generic;
using System.IO;

using Fordere.RestService.Utilities;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.User;

using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;

namespace Fordere.RestService.Extensions
{
    public class UserAuthMetaKeys
    {
        public const string Newsletter = "newsletter";
        public const string DivisionZürich = "division1";
        public const string DivisionStGallen = "division2";
        public const string DivisionLuzern = "division3";
        public const string DivisionWinti = "division4";
    }


    public static class UserAuthExtensions
    {
        public static bool GetHasMeta(this UserAuth user, string key)
        {
            if (user.Meta == null)
            {
                return false;
            }

            return user.Meta.ContainsKey(key);
        }

        public static void SetDivision(this UserAuth user, string key, bool value)
        {
            if (value && (user.Meta == null || !user.Meta.ContainsKey(key)))
            {
                if (user.Meta == null)
                {
                    user.Meta = new Dictionary<string, string>();
                }

                user.Meta.Add(key, "true");
            }
            else if (!value && user.Meta != null && user.Meta.ContainsKey(key))
            {
                user.Meta.Remove(key);
            }
        }

        public static void SetHasNewsletter(this UserAuth user, bool value)
        {
            if (value && (user.Meta == null || !user.Meta.ContainsKey(UserAuthMetaKeys.Newsletter)))
            {
                if (user.Meta == null)
                {
                    user.Meta = new Dictionary<string, string>();
                }

                user.Meta.Add(UserAuthMetaKeys.Newsletter, string.Empty);
            }
            else if (!value && user.Meta != null && user.Meta.ContainsKey(UserAuthMetaKeys.Newsletter))
            {
                user.Meta.Remove(UserAuthMetaKeys.Newsletter);
            }
        }

        public static UserDto ToDto(this UserAuth user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = "{0} {1}".Fmt(user.FirstName, user.LastName).Trim(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.Roles != null && user.Roles.Contains(RoleNames.Admin),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreatedDate = user.CreatedDate
            };
        }

        public static UserDto ToDetailedDto(this UserAuth user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = "{0} {1}".Fmt(user.FirstName, user.LastName).Trim(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.Roles != null && user.Roles.Contains(RoleNames.Admin)
            };
        }

        public static UserProfileResponse ToUserProfileResponse(this UserAuth user)
        {
            var response = user.ConvertTo<UserProfileResponse>();
            response.DivisionLuzern = user.GetHasMeta(UserAuthMetaKeys.DivisionLuzern);
            response.DivisionStGallen = user.GetHasMeta(UserAuthMetaKeys.DivisionStGallen);
            response.DivisionZuerich = user.GetHasMeta(UserAuthMetaKeys.DivisionZürich);
            response.DivisionWinti = user.GetHasMeta(UserAuthMetaKeys.DivisionWinti);
            response.CalendarLink = GetCalendarLink(user.Id);

            return response;
        }

        private static string GetCalendarLink(int userId)
        {
            var appSettings = new AppSettings();
            var pw = appSettings.Get("Calendar.EncPass");

            var calendarIdEnc = userId * 77392;
            return $"/api/v1/calendar/{calendarIdEnc.ToString()}";
        }

        public static string ToDisplay(this UserAuth user)
        {
            return string.Format("{0} {1} (#{2})", user.FirstName, user.LastName, user.Id);
        }
    }
}
