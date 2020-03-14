using System.Collections.Generic;

namespace forderebackend.ServiceModel.Dtos
{
    public class Cup2Dto : NameDto
    {
        public string SeasonId { get; set; }
        public List<NameDto> Teams { get; set; }

        public Cup2Dto()
        {
            Teams = new List<NameDto>();
        }
    }
}