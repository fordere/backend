using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Extensions;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.LeagueRegistration;
using forderebackend.ServiceModel.Messages.TeamInscription;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class TeamInscriptionService : BaseService
    {
        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetTeamInscriptionByIdRequest request)
        {
            return Db.SingleById<TeamInscription>(request.Id).ConvertTo<TeamInscriptionDto>();
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(CreateTeamInscriptionRequest request)
        {
            var teamInscription = request.ConvertTo<TeamInscription>();
            var id = (int) Db.Insert(teamInscription, true);
            return Get(new GetTeamInscriptionByIdRequest {Id = id});
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Put(UpdateTeamInscriptionRequest request)
        {
            var teamInscription = Db.SingleById<TeamInscription>(request.Id);
            teamInscription.Throw404NotFoundIfNull("Competition not found");

            teamInscription.PopulateWith(request);

            Db.Update(teamInscription);

            return Get(new GetTeamInscriptionByIdRequest {Id = request.Id});
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Post(UpdateAssignedLeagueRequest request)
        {
            var inscription = Db.LoadSingleById<TeamInscription>(request.Id);
            inscription.AssignedLeagueId = request.AssignedLeagueId;
            Db.Save(inscription);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(TeamInscriptionsByCompetition request)
        {
            var teamInscriptionEntities = Db.LoadSelect<TeamInscription>(p => p.CompetitionId == request.CompetitionId);
            var barEntities = Db.SelectByIds<Bar>(teamInscriptionEntities.Select(s => s.BarId).Distinct());

            var barsLookup = barEntities.ToDictionary(k => k.Id);

            var dtos = new List<TeamInscriptionDto>(teamInscriptionEntities.Count);

            foreach (var entity in teamInscriptionEntities)
            {
                var dto = entity.ConvertTo<TeamInscriptionDto>();

                var bar = barsLookup[dto.BarId];
                dto.BarName = bar.Name;

                dtos.Add(dto);
            }

            return dtos.OrderBy(o => o.WishLeague).ThenBy(o => o.Name);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(NotAssignedTeamInscriptionsByCompetitionRequest request)
        {
            var competitionId = request.CompetitionId;
            var teamInscriptionEntities =
                Db.LoadSelect<TeamInscription>(p => p.CompetitionId == competitionId && p.AssignedLeagueId == null);
            return teamInscriptionEntities.Select(x => x.ConvertTo<TeamInscriptionDto>());
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public void Delete(DeleteTeamInscriptionRequest request)
        {
            using (var transaction = Db.BeginTransaction())
            {
                var teaminscription = Db.SingleById<TeamInscription>(request.TeamInscriptionId);
                var competition = Db.SingleById<Competition>(teaminscription.CompetitionId);
                long seasonId = competition.SeasonId;

                Db.Delete<Payment>(x =>
                    (x.UserId == teaminscription.Player1Id || x.UserId == teaminscription.Player2Id) &&
                    x.SeasonId == seasonId);

                Db.DeleteById<TeamInscription>(request.TeamInscriptionId);

                transaction.Commit();
            }
        }
    }
}