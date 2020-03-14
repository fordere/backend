using System.Data;
using System.Linq;

using Fordere.RestService.Entities;
using Fordere.RestService.LeagueExecution;
using Fordere.RestService.Properties;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.Cup;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CupService : BaseService
    {
        public object Get(GetCupByIdRequest request)
        {
            return this.Db.LoadSingleById<Cup>(request.Id);
        }

        public object Get(GetAllCupsRequest request)
        {
            return this.Db.LoadSelect<Cup>();
        }

        public object Get(GetCupMatchesRequest request)
        {
            if (request.CupRound.HasValue)
            {
                var matchViews = this.Db.Select(Db.From<MatchView>().Where(p => p.CupId == request.Id && p.CupRound == request.CupRound).OrderBy(o => o.CupRound).ThenBy(o => o.RoundOrder));
                matchViews.ForEach(x => x.GuestTeamIsForfaitOut = false);
                matchViews.ForEach(x => x.HomeTeamIsForfaitOut = false);
                return matchViews.ConvertAll(s => s.ConvertTo<ExtendedMatchDto>());
            }

            var matchViewItems = this.Db.Select(Db.From<MatchView>().Where(p => p.CupId == request.Id).OrderBy(o => o.CupRound).ThenBy(o => o.RoundOrder));
            matchViewItems.ForEach(x => x.GuestTeamIsForfaitOut = false);
            matchViewItems.ForEach(x => x.HomeTeamIsForfaitOut = false);
            return matchViewItems.ConvertAll(s => s.ConvertTo<ExtendedMatchDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(CreateCupFromLeagueRequest request)
        {
            var competition = this.Db.LoadSingleById<Competition>(request.CompetitionId);
            var leagueIds = competition.Leagues.Select(s => s.Id).ToList();
            var teams = this.Db.Select(Db.From<Team>().Where(p => Sql.In(p.LeagueId, leagueIds)));
            var cup = this.Db.SingleById<Cup>(request.Id);

            using (var transaction = this.Db.OpenTransaction(IsolationLevel.RepeatableRead))
            {
                this.Db.Delete(Db.From<Match>().Where(p => p.CupId == request.Id));

                var matches = MatchFactory.CreateCupMatches(teams, competition.Leagues, this.Db);
                matches.ForEach(m => m.CupId = request.Id);

                foreach (var match in matches)
                {
                    this.Db.Insert(match);
                }

                cup.CurrentRound = 1;

                this.Db.Save(cup);

                transaction.Commit();

                return null;
            }
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(CreateCupRoundRequest request)
        {
            using (var transaction = this.Db.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                var cup = this.Db.SingleById<Cup>(request.Id);

                var matchesCurrentRound = this.Db.LoadSelect(Db.From<Match>().Where(p => p.CupId == cup.Id && p.CupRound == cup.CurrentRound));
                var guestTeamIds = matchesCurrentRound.Select(s => s.GuestTeamId).ToList();
                var homeTeamIds = matchesCurrentRound.Select(s => s.HomeTeamId).ToList();
                var teamIds = guestTeamIds.Concat(homeTeamIds).Distinct().ToList();
                var teams = this.Db.SelectByIds<Team>(teamIds);
                var leagueIds = teams.Select(s => s.LeagueId).Distinct().ToList();
                var leagues = this.Db.SelectByIds<League>(leagueIds);

                var newRoundMatches = MatchFactory.CreateNextCupRound(matchesCurrentRound, teams, leagues, this.Db);

                this.Db.Update<Cup>(new { CurrentRound = cup.CurrentRound + 1 }, p => p.Id == cup.Id);

                foreach (var match in newRoundMatches)
                {
                    // we can not use InsertAll as it opens a new transaction -> nested transaction are not supported
                    this.Db.Insert(match);
                }

                transaction.Commit();
            }

            return null;
        }
    }
}
