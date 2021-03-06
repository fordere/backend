using System.Collections.Generic;
using System.Linq;

using Fordere.RestService.Entities;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.LeagueRegistration;

using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    public class EinteilungService : BaseService
    {
        public object Get(EinteilungenRequest request)
        {
            List<IGrouping<League, TeamInscription>> teamInscriptions = Db.LoadSelect(Db.From<TeamInscription>().Where(x => x.CompetitionId == request.CompetitionId)).GroupBy(x => x.AssignedLeague).ToList();

            var dtos = new List<EinteilungLeagueDto>();
            foreach (IGrouping<League, TeamInscription> group in teamInscriptions)
            {
                if (group.Key != null)
                {
                    var dto = new EinteilungLeagueDto();
                    dto.Einteilungen = new List<EinteilungDto>();
                    dto.LeagueGroup = group.Key.Group;
                    dto.LeagueNumber = group.Key.Number;

                    foreach (var items in group)
                    {
                        var itemDto = new EinteilungDto();
                        itemDto.Team = items.Name;
                        itemDto.Player1 = items.Player1.ConvertTo<UserDto>();
                        itemDto.Player2 = items.Player2.ConvertTo<UserDto>();

                        dto.Einteilungen.Add(itemDto);
                    }

                    dtos.Add(dto);
                }
            }

            return dtos.OrderBy(x => x.LeagueNumber).ThenBy(x => x.LeagueGroup); ;
        }
    }
}