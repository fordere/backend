using System;
using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Final;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.Final;
using forderebackend.ServiceModel.Messages.User;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.FinalDay
{
    public class MatchService : BaseService
    {
        private static readonly TimeSpan CacheTime = TimeSpan.FromSeconds(10);

        public object Get(GetRecentFinalDayMatchesRequest request)
        {
            Func<MatchesResponse> func = () =>
            {
                var finalDayCompetitionIds = Db.Select(Db.From<FinalDayCompetition>().Where(x =>
                        x.FinalDayId == request.FinalDayId && x.State == FinalDayCompetitionState.Running))
                    .Select(x => x.Id);
                var playedMatches = Db.Select(Db.From<MatchView>()
                    .Where(x => Sql.In(x.FinalDayCompetitionId, finalDayCompetitionIds) && !x.IsFreeTicket &&
                                x.PlayDate != null && x.ResultDate != null).OrderByDescending(x => x.ResultDate)
                    .Limit(request.Offset, request.PageSize)).ToList();

                var response = CreatePagedResponse<MatchesResponse>(request, playedMatches.Count);
                response.Matches = playedMatches.ConvertAll(x => x.ConvertTo<MatchViewDto>());

                return response;
            };

            return CachedForNonAdmins(func, "FinalDayRecentMatches", CacheTime);
        }

        public object Get(GetUpcommingFinalDayMatchesRequest request)
        {
            Func<MatchesResponse> func = () =>
            {
                var finalDayCompetitionIds = Db.Select(Db.From<FinalDayCompetition>().Where(x =>
                        x.FinalDayId == request.FinalDayId && x.State == FinalDayCompetitionState.Running))
                    .Select(x => x.Id);
                var openMatches = Db.Select(Db.From<MatchView>().Where(x =>
                    Sql.In(x.FinalDayCompetitionId, finalDayCompetitionIds) && x.PlayDate == null)).ToList();

                var matchSorter = new UpcommingMatchSorter();
                var sortedUpcommingMatches = matchSorter.Sort(Db, openMatches, request.FinalDayId);

                var runningMatches = LoadFinalDayRunningMatches(request.FinalDayId);
                sortedUpcommingMatches.RemoveAll(x => runningMatches.Any(r => HasSamePlayer(x, r)));

                var response = CreatePagedResponse<MatchesResponse>(request, sortedUpcommingMatches.Count);
                if (request.PageSize.HasValue)
                    sortedUpcommingMatches = sortedUpcommingMatches.Skip(request.Offset).Take(request.PageSize.Value)
                        .ToList();

                response.Matches = sortedUpcommingMatches.ConvertAll(x => x.ConvertTo<MatchViewDto>());

                return response;
            };

            return CachedForNonAdmins(func, "FinalDayUpcommingMatches", CacheTime);
        }

        public object Get(GetFinishedFinalDayMatchesRequest request)
        {
            Func<MatchesResponse> func = () =>
            {
                var finalDayCompetitionIds =
                    Db.Select(Db.From<FinalDayCompetition>().Where(x => x.FinalDayId == request.FinalDayId))
                        .Select(x => x.Id);
                IEnumerable<MatchView> query = Db.Select(Db.From<MatchView>().Where(x =>
                    Sql.In(x.FinalDayCompetitionId, finalDayCompetitionIds) && x.GuestTeamScore != null &&
                    x.HomeTeamScore != null));

                if (!string.IsNullOrEmpty(request.TeamFilter))
                    query = query.Where(x =>
                        x.HomeTeamName.Contains(request.TeamFilter) || x.GuestTeamName.Contains(request.TeamFilter));

                var finishedMatches = query.ToList();
                var response = CreatePagedResponse<MatchesResponse>(request, finishedMatches.Count);
                if (request.PageSize.HasValue)
                    finishedMatches = finishedMatches.Skip(request.Offset).Take(request.PageSize.Value).ToList();

                response.Matches = finishedMatches.ConvertAll(x => x.ConvertTo<MatchViewDto>());

                return response;
            };

            return CachedForNonAdmins(func, "FinalDayFinishedMatches", CacheTime);
        }

        private static bool HasSamePlayer(MatchView firstMatchView, MatchView secondMatchView)
        {
            var firstMatchViewPlayerIds = new List<int>
            {
                firstMatchView.HomePlayer1Id, firstMatchView.HomePlayer2Id, firstMatchView.GuestPlayer1Id,
                firstMatchView.GuestPlayer2Id
            };
            var secondMatchViewPlayerIds = new List<int>
            {
                secondMatchView.HomePlayer1Id, secondMatchView.HomePlayer2Id, secondMatchView.GuestPlayer1Id,
                secondMatchView.GuestPlayer2Id
            };

            // When we do not have 8 different players in two matches, at least one player doest play in both matches
            return firstMatchViewPlayerIds.Union(secondMatchViewPlayerIds).Count() < 8;
        }

        public object Get(GetRunningFinalDayMatchesRequest request)
        {
            Func<List<MatchView>> func = () => LoadFinalDayRunningMatches(request.FinalDayId);

            return CachedForNonAdmins(func, "FinalDayRunningMatches", CacheTime);
        }

        private List<MatchView> LoadFinalDayRunningMatches(long finalDayId)
        {
            var finalDayCompetitionIds =
                Db.Select(Db.From<FinalDayCompetition>().Where(x => x.FinalDayId == finalDayId)).Select(x => x.Id);

            return Db.Select(Db.From<MatchView>().Where(x =>
                    Sql.In(x.FinalDayCompetitionId, finalDayCompetitionIds) && x.PlayDate != null &&
                    x.ResultDate == null))
                .OrderBy(s => s.FinalDayTableNumber).ToList();
        }
    }
}