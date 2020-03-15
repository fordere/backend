using System;
using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Final;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Dtos.FinalDay;
using forderebackend.ServiceModel.Messages.Final;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface.FinalDay
{
    public class FinalDayCompetitionService : BaseService
    {
        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetFinalDayProgressRequest request)
        {
            var competitions = Db.Select(Db.From<FinalDayCompetition>().Where(x => x.FinalDayId == request.FinalDayId));
            return competitions.Select(GetProgressForCompetition).ToList();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(CreateNewRoundRequest request)
        {
            var currentFinalDayCompetition = Db.SingleById<FinalDayCompetition>(request.FinalDayCompetitionId);
            var competitionMode =
                CompetitionModeFactory.GetCompetitionMode(Db, currentFinalDayCompetition.CompetitionMode);

            var generatedMatches = competitionMode.GenerateMatches(request.FinalDayCompetitionId);
            Db.SaveAll(generatedMatches);
        }

        private FinalDayCompetitionProgressDto GetProgressForCompetition(FinalDayCompetition finalDayCompetition)
        {
            var competitionMode = CompetitionModeFactory.GetCompetitionMode(Db, finalDayCompetition.CompetitionMode);

            var progress = new FinalDayCompetitionProgressDto
            {
                CompetitionState = finalDayCompetition.State,
                CompetitionName = finalDayCompetition.Name,
                CompetitionId = finalDayCompetition.Id,
                CompetitionMode = finalDayCompetition.CompetitionMode,
                MatchesRunning = Db.Count(Db.From<MatchView>().Where(x =>
                    x.FinalDayCompetitionId == finalDayCompetition.Id && x.PlayDate != null && x.ResultDate == null)),
            };

            var finishedMatches = Db.Select(Db.From<MatchView>().Where(x =>
                x.FinalDayCompetitionId == finalDayCompetition.Id &&
                (x.PlayDate != null && x.ResultDate != null || x.IsFreeTicket)));
            progress.MatchesPlayed = finishedMatches.Count;
            progress.MatchesOpen = competitionMode.GetNumberOfMatches(finalDayCompetition.Id) - progress.MatchesPlayed -
                                   progress.MatchesRunning;

            var playedMatches = finishedMatches.Where(x => !x.IsFreeTicket).ToList();

            if (playedMatches.Any())
            {
                progress.AverageMatchDuration = (int) TimeSpan
                    .FromSeconds(playedMatches.Average(x => (x.ResultDate - x.PlayDate).Value.TotalSeconds))
                    .TotalMinutes;

                var numberOfTables = GetNumberOfTablesForFinalDayCompetition(finalDayCompetition.Id);
                if (numberOfTables != 0)
                {
                    double totalDurationInMinutes =
                        progress.AverageMatchDuration * progress.MatchesOpen / numberOfTables;
                    var competitionDuration = TimeSpan.FromMinutes(totalDurationInMinutes);

                    progress.ExpectedEnd = DateTime.Now + competitionDuration;
                }
            }

            return progress;
        }

        private long GetNumberOfTablesForFinalDayCompetition(int finalDayCompetitionId)
        {
            var finalDayCompetition = Db.SingleById<FinalDayCompetition>(finalDayCompetitionId);
            return Db.Count(Db.From<FinalDayTable>().Where(x =>
                x.FinalDayId == finalDayCompetition.FinalDayId && x.TableType == finalDayCompetition.TableType));
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetFinalDayCompetitionRequest request)
        {
            return Db.LoadSingleById<FinalDayCompetition>(request.Id).ConvertTo<FinalDayCompetitionDto>();
        }

        public object Get(GetAllFinalDayCompetitionsRequest request)
        {
            var finalDayCompetitions =
                Db.Select<FinalDayCompetition>(competition => competition.FinalDayId == request.FinalDayId)
                    .OrderBy(competition => competition.Id);
            return finalDayCompetitions.Select(s => s.ConvertTo<FinalDayCompetitionDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetFinishedFinalDayCompetitionsRequest request)
        {
            return Db.Select<FinalDayCompetition>(sql =>
                sql.State == FinalDayCompetitionState.Finished && sql.FinalDayId == request.FinalDayId &&
                sql.CompetitionMode == request.CompetitionMode);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Delete(DeleteFinalDayCompetitionRequest request)
        {
            Db.DeleteById<FinalDayCompetition>(request.Id);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(SaveFinalDayCompetitionRequest request)
        {
            var finalDayCompetition = Db.SingleById<FinalDayCompetition>(request.Id);
            finalDayCompetition.PopulateWith(request);

            Db.Save(finalDayCompetition);

            return Get(new GetFinalDayCompetitionRequest {Id = request.Id});
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(AddFinalDayCompetitionRequest request)
        {
            var newId = Db.Insert(
                new FinalDayCompetition
                {
                    FinalDayId = request.FinalDayId, Name = request.Name, CompetitionMode = request.CompetitionMode,
                    TableType = request.TableType, State = FinalDayCompetitionState.Ready, Priority = request.Priority
                }, true);

            // Wir benötigen immer mindestens 1 Gruppe, das UI des Single KO z.B. erlaubt es nicht eine Gruppe hinzuzufügen!
            Db.Insert(new Group {FinalDayCompetitionId = (int) newId, Number = 1});

            return Get(new GetFinalDayCompetitionRequest {Id = (int) newId});
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(UpdateFinalDayCompetitionStateRequest request)
        {
            var currentFinalDayCompetition = Db.SingleById<FinalDayCompetition>(request.Id);

            switch (request.CompetitionState)
            {
                case FinalDayCompetitionState.OnHold:
                    break;
                case FinalDayCompetitionState.Running:
                    if (currentFinalDayCompetition.State == FinalDayCompetitionState.Ready)
                    {
                        CreateFinalDayMatches(request.Id);
                    }

                    break;
                case FinalDayCompetitionState.Ready:
                    DeleteFinalDayMatches(request.Id);
                    break;
            }

            currentFinalDayCompetition.State = request.CompetitionState;
            Db.Save(currentFinalDayCompetition);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(PlayersInFinalDayCompetitionRequest request)
        {
            var finalDayCompetition = Db.SingleById<FinalDayCompetition>(request.FinalDayCompetitionId);

            var playerInCompetitions = Db.LoadSelect(Db.From<PlayerInFinalDayCompetition>()
                .Where(x => x.FinalDayCompetitionId == request.FinalDayCompetitionId));
            return playerInCompetitions.Select(x => new FinalDayPlayerInCompetitionDto
            {
                Player = x.Player.ConvertTo<UserDto>(),
                Id = x.Id,
                IsActive = x.IsActive
            });
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(TogglePlayerActiveFinalDayCompetition request)
        {
            var playerInFinalDayCompetition =
                Db.SingleById<PlayerInFinalDayCompetition>(request.PlayerInFinalDayCompetitionId);
            playerInFinalDayCompetition.IsActive = !playerInFinalDayCompetition.IsActive;
            Db.Save(playerInFinalDayCompetition);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Delete(DeletePlayerInFinalDayCompetitionRequest request)
        {
            Db.DeleteById<PlayerInFinalDayCompetition>(request.PlayerInFinalDayCompetitionId);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(UpdateFinalDayCompetitionNameRequest request)
        {
            var currentFinalDayCompetition = Db.SingleById<FinalDayCompetition>(request.Id);
            currentFinalDayCompetition.Name = request.Name;
            Db.Save(currentFinalDayCompetition);
        }

        private void DeleteFinalDayMatches(int finalDayCompetitionId)
        {
            Db.Delete<Match>(sql => sql.FinalDayCompetitionId == finalDayCompetitionId);

            var groups = Db.Select(Db.From<Group>().Where(x => x.FinalDayCompetitionId == finalDayCompetitionId));
            var groupIds = groups.Select(x => x.Id);

            Db.Delete(Db.From<CompetitionPlayerStanding>()
                .Where(x => x.FinalDayCompetitionId == finalDayCompetitionId));
            Db.Delete(Db.From<CompetitionTeamStanding>().Where(x => Sql.In(x.GroupId, groupIds)));
        }

        private void CreateFinalDayMatches(int finalDayCompetitionId)
        {
            var currentFinalDayCompetition = Db.SingleById<FinalDayCompetition>(finalDayCompetitionId);
            var competitionMode =
                CompetitionModeFactory.GetCompetitionMode(Db, currentFinalDayCompetition.CompetitionMode);

            var generatedMatches = competitionMode.GenerateMatches(finalDayCompetitionId);
            Db.SaveAll(generatedMatches);

            competitionMode.AfterMatchSafe(generatedMatches, finalDayCompetitionId);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(OvertakeForderePlayerRequest request)
        {
            if (Db.Select(Db.From<PlayerInFinalDayCompetition>().Where(x =>
                    x.FinalDayCompetitionId == request.FinalDayCompetitionId && x.PlayerId == request.PlayerId))
                .Any())
            {
                return null;
            }

            var id = Db.Insert(
                new PlayerInFinalDayCompetition
                {
                    FinalDayCompetitionId = request.FinalDayCompetitionId, PlayerId = request.PlayerId, IsActive = true
                }, true);
            var playerInFinalDayCompetition = Db.LoadSingleById<PlayerInFinalDayCompetition>(id);
            return playerInFinalDayCompetition.ConvertTo<FinalDayPlayerInCompetitionDto>();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(PutTeamOverToFinalDayCompetitionRequest request)
        {
            if (request.FinalDayCompetitionId.HasValue)
            {
                PutTeamOverFromOtherCompetition(request.Id, request.FinalDayCompetitionId.Value);
            }

            if (request.WalkoverInGroupId.HasValue)
            {
                InsertWalkoverInGroup(request.WalkoverInGroupId.Value);
            }

            if (request.LeagueId.HasValue)
            {
                PutOverTeamsFromLeague(request.LeagueId.Value, request.Id);
            }

            if (request.TeamId.HasValue && request.GroupId.HasValue)
            {
                PutOverSpecificTeam(request.TeamId.Value, request.GroupId.Value);
            }
        }

        private void PutTeamOverFromOtherCompetition(int targetFinalDayCompetitionId, int sourceFinalDayCompetitionId)
        {
            var sourceFinalDayCompetition = Db.SingleById<FinalDayCompetition>(sourceFinalDayCompetitionId);
            if (sourceFinalDayCompetition.CompetitionMode != CompetitionMode.Group)
            {
                throw new Exception("Source Final Day Competition must be played in Group-Mode");
            }

            var groups = Db.LoadSelect<Group>(sql => sql.FinalDayCompetitionId == sourceFinalDayCompetitionId);
            var teams = new List<Team>();

            foreach (var sourceGroup in groups)
            {
                var successorTeams = Db.LoadSelect<CompetitionTeamStanding>(sql => sql.GroupId == sourceGroup.Id)
                    .OrderBy(x => x.Rank)
                    .Take(sourceGroup.NumberOfSuccessor)
                    .Select(x => x.Team);
                teams.AddRange(successorTeams);
            }

            var targetGroup = Db.Single<Group>(sql => sql.FinalDayCompetitionId == targetFinalDayCompetitionId);
            if (targetGroup == null)
            {
                var targetGroupId = Db.Insert(new Group {FinalDayCompetitionId = targetFinalDayCompetitionId}, true);
                targetGroup = Db.SingleById<Group>(targetGroupId);
            }

            teams.ForEach(x => PutOverSpecificTeam(x.Id, targetGroup.Id));
        }

        private void InsertWalkoverInGroup(int groupId)
        {
            var teamId = Db.Insert(Team.CreateFreeTicket(), true);

            // TODO cast to int? Was wenn forder eunendlich gross wird? :D
            PutOverSpecificTeam((int) teamId, groupId);
        }

        private void PutOverTeamsFromLeague(int leagueId, int targetFinalDayCompetitionId)
        {
            var groups = Db.Select<Group>(x => x.FinalDayCompetitionId == targetFinalDayCompetitionId);
            var league = Db.SingleById<League>(leagueId);
            var allLeagues =
                Db.LoadSelect<League>(x => x.Number == league.Number && x.CompetitionId == league.CompetitionId);

            // TODO ssh: Richtige setzung berechnen
            var allTeams = allLeagues.SelectMany(x => x.Teams)
                .Where(x => x.QualifiedForFinalDay == QualifiedForFinalDay.Yes).ToList();

            if (groups.Count == 1)
            {
                for (var index = 0; index < allTeams.Count; index++)
                {
                    var team = allTeams[index];
                    Db.Insert(new TeamInGroup {TeamId = team.Id, GroupId = groups.Single().Id, Settlement = index + 1});
                }
            }
            else
            {
                for (var index = 0; index < allTeams.Count; index++)
                {
                    var team = allTeams[index];
                    var groupIndex = index % groups.Count;
                    Db.Insert(new TeamInGroup
                        {TeamId = team.Id, GroupId = groups[groupIndex].Id, Settlement = index + 1});
                }
            }

            UpdateSettlementForGroups(targetFinalDayCompetitionId);
        }

        private void PutOverSpecificTeam(int teamId, int groupId)
        {
            var maxSettlementInGroup = 0;
            var existingTeamsInGroup = Db.Select<TeamInGroup>(x => x.GroupId == groupId);
            if (existingTeamsInGroup.Any())
            {
                maxSettlementInGroup = existingTeamsInGroup.Max(x => x.Settlement);
            }

            var teamInGroup = new TeamInGroup
                {TeamId = teamId, GroupId = groupId, Settlement = maxSettlementInGroup + 1};
            Db.Insert(teamInGroup);
        }

        private void UpdateSettlementForGroups(int finalDayCompetitionId)
        {
            var groups = Db.LoadSelect<Group>(sql => sql.FinalDayCompetitionId == finalDayCompetitionId);
            foreach (var competitionGroup in groups)
            {
                var orderedTeamsInGroup = competitionGroup.Teams.OrderBy(x => x.Settlement).ToList();

                for (var i = 0; i < orderedTeamsInGroup.Count; i++) orderedTeamsInGroup[i].Settlement = i + 1;

                Db.SaveAll(orderedTeamsInGroup);
            }
        }
    }
}