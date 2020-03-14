using System.Collections.Generic;

namespace forderebackend.ServiceModel.Dtos
{
    public class CupDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SeasonId { get; set; }

        public SeasonDto Season { get; set; }

        public int CurrentRound { get; set; }

        public List<ExtendedMatchDto> Matches { get; set; }
    }
}