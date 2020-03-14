using System.Data;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.LeagueExecution;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.Cup;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class CupService : BaseService
    {
        public object Get(GetCupByIdRequest request)
        {
            return Db.LoadSingleById<Cup>(request.Id);
        }

        public object Get(GetAllCupsRequest request)
        {
            return Db.LoadSelect<Cup>();
        }

        public object Get(GetCupMatchesRequest request)
        {
            if (request.CupRound.HasValue)
            {
                var matchViews = Db.Select(Db.From<MatchView>()
                    .Where(p => p.CupId == request.Id && p.CupRound == request.CupRound).OrderBy(o => o.CupRound)
                    .ThenBy(o => o.RoundOrder));
                matchViews.ForEach(x => x.GuestTeamIsForfaitOut = false);
                matchViews.ForEach(x => x.HomeTeamIsForfaitOut = false);
                return matchViews.ConvertAll(s => s.ConvertTo<ExtendedMatchDto>());
            }

            var matchViewItems = Db.Select(Db.From<MatchView>().Where(p => p.CupId == request.Id)
                .OrderBy(o => o.CupRound).ThenBy(o => o.RoundOrder));
            matchViewItems.ForEach(x => x.GuestTeamIsForfaitOut = false);
            matchViewItems.ForEach(x => x.HomeTeamIsForfaitOut = false);
            return matchViewItems.ConvertAll(s => s.ConvertTo<ExtendedMatchDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(CreateCupFromLeagueRequest request)
        {
            var competition = Db.LoadSingleById<Competition>(request.CompetitionId);
            var leagueIds = competition.Leagues.Select(s => s.Id).ToList();
            var teams = Db.Select(Db.From<Team>().Where(p => Sql.In(p.LeagueId, leagueIds)));
            var cup = Db.SingleById<Cup>(request.Id);

            using (var transaction = Db.OpenTransaction(IsolationLevel.RepeatableRead))
            {
                Db.Delete(Db.From<Match>().Where(p => p.CupId == request.Id));

                var matches = MatchFactory.CreateCupMatches(teams, competition.Leagues, Db);
                matches.ForEach(m => m.CupId = request.Id);

                foreach (var match in matches) Db.Insert(match);

                cup.CurrentRound = 1;

                Db.Save(cup);

                transaction.Commit();

                return null;
            }
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(CreateCupRoundRequest request)
        {
            using (var transaction = Db.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                var cup = Db.SingleById<Cup>(request.Id);

                var matchesCurrentRound =
                    Db.LoadSelect(Db.From<Match>().Where(p => p.CupId == cup.Id && p.CupRound == cup.CurrentRound));
                var guestTeamIds = matchesCurrentRound.Select(s => s.GuestTeamId).ToList();
                var homeTeamIds = matchesCurrentRound.Select(s => s.HomeTeamId).ToList();
                var teamIds = guestTeamIds.Concat(homeTeamIds).Distinct().ToList();
                var teams = Db.SelectByIds<Team>(teamIds);
                var leagueIds = teams.Select(s => s.LeagueId).Distinct().ToList();
                var leagues = Db.SelectByIds<League>(leagueIds);

                var newRoundMatches = MatchFactory.CreateNextCupRound(matchesCurrentRound, teams, leagues, Db);

                Db.Update<Cup>(new {CurrentRound = cup.CurrentRound + 1}, p => p.Id == cup.Id);

                foreach (var match in newRoundMatches)
                    // we can not use InsertAll as it opens a new transaction -> nested transaction are not supported
                    Db.Insert(match);

                transaction.Commit();
            }

            return null;
        }
    }
}