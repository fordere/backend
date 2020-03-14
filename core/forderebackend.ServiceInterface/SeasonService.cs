using System;
using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.Season;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class SeasonService : BaseService
    {
        public object Get(GetSeasonById request)
        {
            return Db.LoadSingleById<Season>(request.Id);
        }

        public object Get(GetCurrentSeasonRequest request)
        {
            // TODO SSH Maybe we have to handle that somehow different
            var currentSeasons = Db.LoadSelect(Db.From<Season>()
                .Where(x => x.State != SeasonState.Archived && x.DivisionId == DivisionId));

            if (currentSeasons.Count == 1)
            {
                var season = currentSeasons.Single();
                return MapToSeasonDto(season);
            }

            if (currentSeasons.Count == 0)
            {
                return null;
            }

            if (currentSeasons.Any(x => x.State == SeasonState.Running))
            {
                return MapToSeasonDto(currentSeasons.First(x => x.State == SeasonState.Running));
            }

            return MapToSeasonDto(currentSeasons.First());
        }

        private static SeasonDto MapToSeasonDto(Season season)
        {
            var seasonDto = season.ConvertTo<SeasonDto>();
            seasonDto.Competitions = season.Competitions?.ConvertAll(x => x.ConvertTo<CompetitionDto>()) ??
                                     new List<CompetitionDto>();
            return seasonDto;
        }

        public object Get(GetAllSeasonsRequest request)
        {
            var seasons = Db.LoadSelect(Db.From<Season>().Where(x => x.DivisionId == DivisionId).OrderBy(o => o.Id));
            return seasons.Select(s => s.ConvertTo<SeasonDto>());
        }

        public object Get(GetAllArchivedSeasonsRequest request)
        {
            var seasons = Db.LoadSelect(Db.From<Season>()
                .Where(x => x.State == SeasonState.Archived && x.DivisionId == DivisionId).OrderBy(x => x.Id));
            return seasons.Select(s => s.ConvertTo<SeasonDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetLeaguesBySeasonRequest request)
        {
            var seasonCompetitionIds = Db.Select<Competition>(sql => sql.SeasonId == request.SeasonId).Select(x => x.Id)
                .ToList();

            // TODO SSH: Kann das nicht als "Any(..)" im sql geschrieben werden?
            var leagues = new List<League>();
            foreach (var seasonCompetitionId in seasonCompetitionIds)
            {
                var competitionLeagues =
                    Db.LoadSelect<League>(sql => sql.CompetitionId == seasonCompetitionId).ToList();
                var groupedLeagues = competitionLeagues.GroupBy(x => x.Number);
                leagues.AddRange(groupedLeagues.Select(groupedLeague => groupedLeague.First()));
            }

            return leagues.Select(x => x.ConvertTo<LeagueDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(SaveSeasonRequest request)
        {
            var season = request.ConvertTo<Season>();

            if (DivisionId.HasValue)
            {
                season.DivisionId = DivisionId.Value;
            }
            else
            {
                throw new ArgumentException("No Division ID set on Season manipulation");
            }

            if (season.Id == 0)
            {
                var seasonId = (int) Db.Insert(season, true);
                Db.Insert(new Entities.Final.FinalDay {SeasonId = seasonId});
            }
            else
            {
                Db.Save(season);
            }

            return season.ConvertTo<SeasonDto>();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetAllTeamsInSeasonRequest request)
        {
            var competitionIds = Db.Select<Competition>(sql => sql.SeasonId == request.SeasonId).Select(x => x.Id);
            return Db.Select(Db.From<TeamView>().Where(sql => Sql.In(sql.CompetitionId, competitionIds))
                .OrderBy(x => x.Name)).Select(x => x.ConvertTo<TeamViewDto>());
        }
    }
}