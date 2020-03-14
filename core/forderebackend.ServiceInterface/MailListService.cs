using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.MailingList;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class MailListService : BaseService
    {
        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetAllPlayersInSeasonMailsRequest request)
        {
            var competitions = Db.Select<Competition>(sql => sql.SeasonId == request.SeasonId);
            var mailList = competitions.SelectMany(x => GetAllUserMailsInCompetitionForInscriptions(x.Id)).Distinct();
            return new UserMailsDto(mailList);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetAllPlayersInLeagueMailsRequest request)
        {
            var mailList = GetAllUserMailsInLeague(request.LeagueId);
            return new UserMailsDto(mailList);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetAllUsersMailsRequest request)
        {
            //var competitionssg = Db.Select<Competition>(sql => sql.SeasonId == 22);
            //var mailListSG = competitionssg.SelectMany(x => GetAllUserMailsInCompetitionForInscriptions(x.Id)).Distinct();


            //var competitionslu = Db.Select<Competition>(sql => sql.SeasonId == 21);
            //var mailListlu = competitionslu.SelectMany(x => GetAllUserMailsInCompetitionForInscriptions(x.Id)).Distinct();


            //var allUsers = Db.Select<UserAuth>().ToList();

            //foreach (var user in allUsers)
            //{
            //    if (user.CreatedDate < new System.DateTime(2017, 7, 7))
            //    {
            //        if (!user.Meta.ContainsKey("division1"))
            //        {
            //            user.Meta.Add("division1", "true");
            //        }
            //        continue;
            //    }

            //    if (mailListlu.Any(x => x.Id == user.Id))
            //    {
            //        if (!user.Meta.ContainsKey("division3"))
            //        {
            //            user.Meta.Add("division3", "true");
            //        }
            //    }
            //    else if (mailListSG.Any(x => x.Id == user.Id))
            //    {
            //        if (!user.Meta.ContainsKey("division2"))
            //        {
            //            user.Meta.Add("division2", "true");
            //        }
            //    }
            //    else
            //    {
            //        if (!user.Meta.ContainsKey("division1"))
            //        {
            //            user.Meta.Add("division1", "true");
            //        }
            //    }

            //}

            //Db.SaveAll<UserAuth>(allUsers);


            var mailList = Db.Select<UserAuth>().Where(x =>
                x.Email != null && x.Meta != null && x.Meta.ContainsKey("division" + DivisionId) &&
                x.Meta["division" + DivisionId] == "true" && !x.Email.EndsWith("@fordere.ch"));
            return new UserMailsDto(mailList);
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Get(GetAllPlayersInCompetitionMailsRequest request)
        {
            var mailList = GetAllUserMailsInCompetition(request.CompetitionId);
            return new UserMailsDto(mailList);
        }

        private IEnumerable<UserAuth> GetAllUserMailsInLeague(int leagueId)
        {
            var teams = Db.Select<Team>(sql => sql.LeagueId == leagueId);
            var userIds = teams.Select(x => x.Player1Id).Concat(teams.Select(x => x.Player2Id)).Distinct();

            var mailList = Db.SelectByIds<UserAuth>(userIds);
            return mailList;
        }

        private IEnumerable<UserAuth> GetAllUserMailsInCompetition(int competitionId)
        {
            var leagues = Db.Select<League>(sql => sql.CompetitionId == competitionId);
            return leagues.SelectMany(x => GetAllUserMailsInLeague(x.Id));
        }

        private IEnumerable<UserAuth> GetAllUserMailsInCompetitionForInscriptions(int competitionId)
        {
            var inscriptions = Db.Select<TeamInscription>(sql => sql.CompetitionId == competitionId);
            var userIds = inscriptions.Select(x => x.Player1Id).Concat(inscriptions.Select(x => x.Player2Id))
                .Distinct();

            var mailList = Db.SelectByIds<UserAuth>(userIds);
            return mailList;
        }
    }
}