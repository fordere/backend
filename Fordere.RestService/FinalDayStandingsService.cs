using System;
using System.Collections.Generic;
using System.Linq;

using Fordere.RestService.Entities;
using Fordere.RestService.Entities.Final;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.Final;
using Fordere.ServiceInterface.Messages.Season;

using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    public class FinalDayStandingsService : BaseService
    {
        public object Get(GetFinalDayStandingsRequest request)
        {
            Season season;
            if (request.SeasonId.HasValue)
            {
                season = Db.LoadSingleById<Season>(request.SeasonId.Value);
            }
            else
            {
                season = Db.LoadSelect(Db.From<Season>().Where(x => x.State != SeasonState.Archived && x.DivisionId == DivisionId)).SingleOrDefault();
            }

            var competitions = Db.Select(Db.From<FinalDayCompetition>().Where(x => x.FinalDayId == season.FinalDay.Id));
            var competitionStandingDtos = new List<CompetitionStandingDto>();

            foreach (var competition in competitions)
            {
                switch (competition.CompetitionMode)
                {
                    case CompetitionMode.SingleKO:
                        competitionStandingDtos.Add(LoadSingleKoStandings(competition));
                        break;
                    case CompetitionMode.Group:
                        competitionStandingDtos.Add(LoadGroupStandings(competition));
                        break;
                    case CompetitionMode.DoubleKO:
                        throw new NotSupportedException("Irgendwer sollte DoubleKO noch implementieren!");
                    case CompetitionMode.CrazyDyp:
                        competitionStandingDtos.Add(LoadCrazyDypStandings(competition));
                        break;
                    case CompetitionMode.Dyp:
                        throw new NotSupportedException("Irgendwer sollte DoubleKO noch implementieren!");

                }
            }

            return competitionStandingDtos;
        }

        private CompetitionStandingDto LoadCrazyDypStandings(FinalDayCompetition competition)
        {
            var compDto = new CompetitionStandingDto
            {
                CompetitionId = competition.Id,
                CompetitionName = competition.Name,
                CompetitionMode = competition.CompetitionMode,
                CompetitionState = competition.State
            };

            var sqlExpression = Db.From<CompetitionPlayerStanding>()
               .Where(x => x.FinalDayCompetitionId == competition.Id)
               .ThenBy(x => x.Rank);

            var standings = Db.LoadSelect(sqlExpression).ToList();
            compDto.Players = standings.Select(x => x.ConvertTo<CompetitionPlayerStandingDto>()).ToList();
            return compDto;
        }

        private CompetitionStandingDto LoadSingleKoStandings(FinalDayCompetition competition)
        {
            var matches = Db.Select(Db.From<MatchView>().Where(x => x.FinalDayCompetitionId == competition.Id).OrderBy(x => x.CupRound).ThenBy(x => x.RoundOrder)).ToList();
            var dtos = matches.ConvertAll(s => s.ConvertTo<ExtendedMatchDto>());

            return new CompetitionStandingDto
            {
                CompetitionId = competition.Id,
                CompetitionName = competition.Name,
                CompetitionMode = competition.CompetitionMode,
                CompetitionState = competition.State,
                SingleKoMatches = dtos
            };

        }

        private CompetitionStandingDto LoadGroupStandings(FinalDayCompetition competition)
        {
            // TODO SSH: Refactor this -> Duplicated code just above...
            var compDto = new CompetitionStandingDto
            {
                CompetitionId = competition.Id,
                CompetitionName = competition.Name,
                CompetitionMode = competition.CompetitionMode,
                CompetitionState = competition.State
            };

            var sqlExpression = Db.From<CompetitionTeamStandingView>()
               .Where(x => x.FinalDayCompetitionId == competition.Id)
               .OrderBy(x => x.GroupId)
               .ThenBy(x => x.Rank);

            var standings = Db.Select(sqlExpression).ToList();
            compDto.Groups = MapToCompetitionTeamStandingDto(standings);
            return compDto;
        }

        // TODO SSH: Does this grouping stuff make sense here? Could we use the normal DB infrastructore?
        private static List<CompetitionTeamStandingGroupDto> MapToCompetitionTeamStandingDto(List<CompetitionTeamStandingView> standings)
        {
            var groupDtos = new List<CompetitionTeamStandingGroupDto>();
            var groups = standings.GroupBy(x => x.GroupId);
            foreach (var finalDayCompetitionGroup in groups)
            {
                var firstItem = finalDayCompetitionGroup.First();
                var groupDto = new CompetitionTeamStandingGroupDto
                {
                    Number = firstItem.GroupNumber,
                    NumberOfSuccessor = firstItem.GroupNumberOfSuccessor
                };

                groupDto.Standings.AddRange(finalDayCompetitionGroup.ToList().ConvertAll(x => x.ConvertTo<CompetitionTeamStandingViewDto>()));
                groupDtos.Add(groupDto);
            }

            return groupDtos;

        }
    }
}