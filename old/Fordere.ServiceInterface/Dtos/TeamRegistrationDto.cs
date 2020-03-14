using System.Collections.Generic;

namespace Fordere.ServiceInterface.Dtos
{
    public class TeamRegistrationDto
    {
        public string Name { get; set; }
        public List<string> UserIds { get; set; }
        public int? LeagueWish { get; set; }

        public TeamRegistrationDto()
        {
            this.UserIds = new List<string>();
        }
    }
}