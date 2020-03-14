using System.Collections.Generic;
using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.LeagueRegistration;
using ServiceStack;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class EinteilungService : BaseService
    {
        public object Get(EinteilungenRequest request)
        {
            var teamInscriptions =
                Db.LoadSelect(Db.From<TeamInscription>().Where(x => x.CompetitionId == request.CompetitionId))
                    .GroupBy(x => x.AssignedLeague).ToList();

            var dtos = new List<EinteilungLeagueDto>();
            foreach (var group in teamInscriptions)
                if (@group.Key != null)
                {
                    var dto = new EinteilungLeagueDto();
                    dto.Einteilungen = new List<EinteilungDto>();
                    dto.LeagueGroup = @group.Key.Group;
                    dto.LeagueNumber = @group.Key.Number;

                    foreach (var items in @group)
                    {
                        var itemDto = new EinteilungDto();
                        itemDto.Team = items.Name;
                        itemDto.Player1 = items.Player1.ConvertTo<UserDto>();
                        itemDto.Player2 = items.Player2.ConvertTo<UserDto>();

                        dto.Einteilungen.Add(itemDto);
                    }

                    dtos.Add(dto);
                }

            return dtos.OrderBy(x => x.LeagueNumber).ThenBy(x => x.LeagueGroup);
            ;
        }
    }
}