using System.Collections.Generic;

namespace forderebackend.ServiceModel.Dtos
{
    public class CompetitionDto : NameDto
    {
        public SeasonDto Season { get; set; }

        public List<LeagueDto> Leagues { get; set; }

        public string RegistrationText { get; set; }

        public string Rules { get; set; }

        public string Modus { get; set; }
    }
}