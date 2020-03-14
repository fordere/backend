using System;
using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class StatisticsService : BaseService
    {
        public object Get(GetAllStatisticsRequest request)
        {
            var matches = Db.Select<MatchView>(x => x.SeasonId == request.SeasonId);

            var forfaitMatches =
                matches.Count(x => (x.HomeTeamIsForfaitOut || x.GuestTeamIsForfaitOut) && !x.BarId.HasValue);
            var playedMatches = matches.Count(x =>
                x.PlayDate != null && x.PlayDate < DateTime.Now && x.BarId.HasValue && !x.IsNotPlayedMatch &&
                x.HasResult);
            var bookedMatches = matches.Count(x =>
                !x.HomeTeamIsForfaitOut && !x.GuestTeamIsForfaitOut && x.PlayDate > DateTime.Now && x.BarId.HasValue &&
                !x.IsNotPlayedMatch);
            var notPlayedMatches =
                matches.Count(x => x.IsNotPlayedMatch && !(x.HomeTeamIsForfaitOut || x.GuestTeamIsForfaitOut));
            var openMatchCount = matches.Count - forfaitMatches - playedMatches - bookedMatches - notPlayedMatches;

            var matchesStates = new DonoughtStatistic
            {
                Data = new List<int> {playedMatches, bookedMatches, openMatchCount, forfaitMatches, notPlayedMatches},
                Labels = new List<string> {"Gespielt", "Abgemacht", "Offen", "Forfait", "Nicht gespielt"}
            };

            var matchesInBar = new Dictionary<long, int>();
            var matchesInWeek = new Dictionary<string, int>();
            // Only used for german translation, i.e. Monday -> Montag etc.
            var weekdayTranslation = new Dictionary<DayOfWeek, string>()
            {
                {DayOfWeek.Monday, "Montag"},
                {DayOfWeek.Tuesday, "Dienstag"},
                {DayOfWeek.Wednesday, "Mittwoch"},
                {DayOfWeek.Thursday, "Donnerstag"},
                {DayOfWeek.Friday, "Freitag "},
                {DayOfWeek.Saturday, "Samstag"},
                {DayOfWeek.Sunday, "Sonntag"}
            };
            var matchesPerWeekday = new Dictionary<string, int>()
            {
                // TODO: This is a hack to make sure the order of the weekdays is correct (the order is the same as the order in which the keys are added to the dict).
                // We can probably streamline this a bit... ;)
                {weekdayTranslation[DayOfWeek.Monday], 0},
                {weekdayTranslation[DayOfWeek.Tuesday], 0},
                {weekdayTranslation[DayOfWeek.Wednesday], 0},
                {weekdayTranslation[DayOfWeek.Thursday], 0},
                {weekdayTranslation[DayOfWeek.Friday], 0},
                {weekdayTranslation[DayOfWeek.Saturday], 0},
                {weekdayTranslation[DayOfWeek.Sunday], 0}
            };

            var allPlayedMatches =
                Db.LoadSelect<MatchView>(sql =>
                        sql.SeasonId == request.SeasonId && sql.PlayDate < DateTime.Now && sql.BarId != null)
                    .OrderBy(x => x.PlayDate).ToList();
            foreach (var match in allPlayedMatches)
            {
                if (matchesInBar.ContainsKey(match.BarId.Value))
                    matchesInBar[match.BarId.Value]++;
                else
                    matchesInBar.Add(match.BarId.Value, 1);

                var playDate = string.Format("{0:dd.MM.yyyy}", GetMonday(match.PlayDate));
                if (matchesInWeek.ContainsKey(playDate))
                    matchesInWeek[playDate]++;
                else
                    matchesInWeek.Add(playDate, 1);

                ++matchesPerWeekday[weekdayTranslation[match.PlayDate.Value.DayOfWeek]];
            }

            var matchInBarStats = new DonoughtStatistic
            {
                Data = matchesInBar.Values.ToList(),
                Labels = matchesInBar.Keys.Select(x => Db.Select<Bar>(sql => sql.Id == x).Single().Name).ToList()
            };

            var matchesPerWeek = new LineStatistic
            {
                Series = new List<string> {"Liga"}, Data = new List<List<int>> {matchesInWeek.Values.ToList()},
                Labels = matchesInWeek.Keys.ToList()
            };

            var matchesPerWeekdayStats = new LineStatistic()
            {
                Data = new List<List<int>>() {matchesPerWeekday.Values.ToList()},
                Labels = matchesPerWeekday.Keys.ToList()
            };

            return new StatisticsDto
            {
                MatchesPlayed = matchesStates, MatchesPerLocation = matchInBarStats, MatchesPerWeek = matchesPerWeek,
                MatchesPerWeekday = matchesPerWeekdayStats
            };
        }

        private DateTime GetMonday(DateTime? din)
        {
            var d = din.Value;
            var offset = d.DayOfWeek - DayOfWeek.Monday;
            return d.AddDays(-offset);
        }
    }
}