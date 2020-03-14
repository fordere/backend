using System.Collections.Generic;

namespace forderebackend.ServiceModel.Dtos
{
    public class TeamRegistrationDto
    {
        public string Name { get; set; }
        public List<string> UserIds { get; set; }
        public int? LeagueWish { get; set; }

        public TeamRegistrationDto()
        {
            UserIds = new List<string>();
        }
    }
}