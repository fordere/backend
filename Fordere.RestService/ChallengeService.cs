using System.Collections.Generic;
using System.Linq;

using Fordere.RestService.Entities;
using Fordere.RestService.Properties;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.Challenge;

using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ChallengeService : BaseService
    {
        public object Get(GetChallengesForLeague request)
        {
            var challenges = this.Db.LoadSelect(Db.From<Challenge>().Where(p => p.LeagueId == request.LeagueId));
            var teamIds = challenges.Select(s => s.ChallengingTeamId).Union(challenges.Select(s => s.AcceptingTeamId)).Distinct().ToList();
            var teamLookup = this.Db.SelectByIds<Team>(teamIds).ToDictionary(k => k.Id);

            var dtos = new List<ChallengeDto>(challenges.Count);

            foreach (var challenge in challenges)
            {
                var dto = challenge.ConvertTo<ChallengeDto>();
                dto.ChallengingTeam = teamLookup[challenge.ChallengingTeamId].ConvertTo<NameDto>();

                dtos.Add(dto);
            }

            return dtos;
        }

        public void Post(CreateChallengeRequest request)
        {
            var team = this.Db.SingleById<Team>(request.TeamId);

            var challenge = new Challenge
            {
                ChallengingTeamId = request.TeamId,
                ProposedDate = request.ProposedDate,
                LeagueId = team.LeagueId.GetValueOrDefault(),
                TableId = request.TableId,
            };

            this.Db.Insert(challenge);
        }
    }
}