﻿using System.Collections.Generic;



namespace Fordere.ServiceInterface.Dtos
{
    
    public class OpenRegistrationCompetitionDto : NameDto
    {
        public SeasonDto Season { get; set; }

        public List<LeagueDto> Leagues { get; set; }

        public string RegistrationText { get; set; }

        public TeamInscriptionDto CurrentUserTeamInscription { get; set; }
    }
}