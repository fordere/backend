using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;

namespace Fordere.ServiceInterface.Dtos
{
    [UsedImplicitly]
    public class OpenRegistrationCompetitionDto : NameDto
    {
        public SeasonDto Season { get; set; }

        public List<LeagueDto> Leagues { get; set; }

        public string RegistrationText { get; set; }

        public TeamInscriptionDto CurrentUserTeamInscription { get; set; }
    }
}