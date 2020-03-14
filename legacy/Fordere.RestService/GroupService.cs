using System.Collections.Generic;
using System.Linq;

using Fordere.RestService.Entities.Final;
using Fordere.ServiceInterface.Dtos.FinalDay;
using Fordere.ServiceInterface.Messages.Final;

using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    public class GroupService : BaseService
    {
        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Delete(DeleteGroupRequest request)
        {
            Db.DeleteById<Group>(request.GroupId);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Delete(DeleteTeamFromGroupRequest request)
        {
            var teamInGroup = Db.SingleById<TeamInGroup>(request.TeamInGroupId);

            var teamsToUpdate = Db.Select<TeamInGroup>(x => x.Settlement > teamInGroup.Settlement && x.GroupId == teamInGroup.GroupId);
            teamsToUpdate.ForEach(x => x.Settlement--);
            Db.SaveAll(teamsToUpdate);

            Db.DeleteById<TeamInGroup>(request.TeamInGroupId);

            var updatedTeamIs = teamsToUpdate.Select(x => x.Id);
            var updatedTeams = Db.Select<TeamInGroupView>(sql => Sql.In(sql.Id, updatedTeamIs));
            return updatedTeams.ConvertAll(x => x.ConvertTo<TeamInGroupViewDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(MoveTeamInGroupRequest request)
        {
            var sourceTeamInGroup = Db.SingleById<TeamInGroup>(request.TeamInGroupId);
            var teamsToMoveSource = Db.Select<TeamInGroup>(sql => sql.GroupId == sourceTeamInGroup.GroupId && sql.Settlement >= sourceTeamInGroup.Settlement);
            teamsToMoveSource.ForEach(x => x.Settlement--);
            Db.SaveAll(teamsToMoveSource);

            var teamsToMoveTarget = Db.Select<TeamInGroup>(sql => sql.GroupId == request.TargetGroupId && sql.Settlement >= request.TargetSettlement);
            teamsToMoveTarget.ForEach(x => x.Settlement++);
            Db.SaveAll(teamsToMoveTarget);

            var teamInGroup = Db.SingleById<TeamInGroup>(request.TeamInGroupId);
            teamInGroup.Settlement = request.TargetSettlement;
            teamInGroup.GroupId = request.TargetGroupId;
            Db.Save(teamInGroup);

            var updatedTeamIds = teamsToMoveSource.Concat(teamsToMoveTarget).Select(x => x.Id).Distinct();

            var updatedTeams = Db.Select<TeamInGroupView>(sql => Sql.In(sql.Id, updatedTeamIds));
            return updatedTeams.ConvertAll(x => x.ConvertTo<TeamInGroupViewDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(SaveGroupRequest request)
        {
            var existing = Db.SingleById<Group>(request.GroupId);
            var updated = existing.PopulateWith(request);
            Db.Save(updated);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(AddGroupRequest request)
        {
            var existingGroups = Db.Select<Group>(x => x.FinalDayCompetitionId == request.FinalDayCompetitionId);
            int nextGroupNumber = 1;

            if (existingGroups.Any())
            {
                nextGroupNumber = existingGroups.Max(x => x.Number) + 1;
            }

            var groupToAdd = new Group { Number = nextGroupNumber, FinalDayCompetitionId = request.FinalDayCompetitionId };
            var groupId = Db.Insert(groupToAdd, true);

            return Db.SingleById<Group>(groupId).ConvertTo<GroupDto>();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetGroupsByCompetitionRequest request)
        {
            var groups = Db.Select<Group>(sql => sql.FinalDayCompetitionId == request.FinalDayCompetitionId).ToList();
            foreach (var competitionGroup in groups)
            {
                competitionGroup.Teams = Db.LoadSelect<TeamInGroup>(sql => sql.GroupId == competitionGroup.Id).ToList();
            }

            var groupDtos = new List<GroupDto>();
            foreach (var competitionGroup in groups)
            {
                var groupDto = competitionGroup.ConvertTo<GroupDto>();
                var teams = Db.Select<TeamInGroupView>(sql => sql.GroupId == groupDto.Id);
                groupDto.Teams = teams.ConvertAll(x => x.ConvertTo<TeamInGroupViewDto>());
                groupDtos.Add(groupDto);
            }

            return groupDtos;
        }

    }
}