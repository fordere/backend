using System;
using System.IO;
using System.Text;

using Fordere.RestService.Entities;
using Fordere.RestService.Properties;
using Fordere.RestService.Utilities;
using Fordere.ServiceInterface.Messages.Calendar;

using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class CalendarService : BaseService
    {
        [AddHeader(ContentType = "text/calendar; charset=UTF-8")]
        public Stream Get(GetCalendarStreamByIdRequest request)
        {
            var calendarIdEnc = request.Id;//;.ReplaceAll("_", "/");

            var appSettings = new AppSettings();
            var pw = appSettings.Get("Calendar.EncPass");

            int userId;
            try
            {
                userId = int.Parse(calendarIdEnc) / 77392; // StringCipher.Decrypt(calendarIdEnc, pw);
            }
            catch (Exception e)
            {
                return new MemoryStream();
            }

            var matches = Db.Select<MatchView>(p => p.PlayDate.HasValue && p.PlayDate >= DateTime.Today && (p.GuestPlayer1Id == userId || p.GuestPlayer2Id == userId || p.HomePlayer1Id == userId || p.HomePlayer2Id == userId));


            var calendar = new Calendar();

            var user = Db.LoadSingleById<UserAuth>(userId);

            var calendarName = $"Fordere {user.FirstName} {user.LastName}";
            if (userId == 107)
            {
                calendarName = "Fordere Flo - Dümmscht - Schwendener";
            }
            calendar.Properties.Add(new CalendarProperty("X-WR-CALNAME", calendarName));
            calendar.Properties.Add(new CalendarProperty("CALSCALE", "GREGORIAN"));
            calendar.Properties.Add(new CalendarProperty("METHOD", "PUBLISH"));
            calendar.Properties.Add(new CalendarProperty("X-PUBLISHED-TTL", "PT12H"));
           // calendar.Properties.Add(new CalendarProperty("X-WR-TIMEZONE", "PUBLISH"));


            foreach (var match in matches)
            {
                var e = new CalendarEvent
                {
                    Summary = $"{match.HomeTeamName} vs. {match.GuestTeamName}",
                    IsAllDay = false,
                    Organizer = new Organizer
                    {
                        CommonName = "fordere.ch",
                        Value = new Uri("https://www.fordere.ch")
                    },
                    Start = new CalDateTime(match.PlayDate.Value.ToUniversalTime()),
                    End = new CalDateTime(match.PlayDate.Value.AddMinutes(30).ToUniversalTime()),
                    Location = match.BarName,
                    Description = match.CompetitionName,
                };
                calendar.Events.Add(e);
            }

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            var bytesCalendar = Encoding.UTF8.GetBytes(serializedCalendar);
            var ms = new MemoryStream(bytesCalendar);
            return ms;
        }
    }
}
