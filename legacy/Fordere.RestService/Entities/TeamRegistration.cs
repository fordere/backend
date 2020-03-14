using System.Collections.Generic;

namespace Fordere.RestService.Entities
{
    public class TeamRegistration
    {
        public string Name { get; set; }
        public List<int> UserAuthIds { get; set; }
        public int? LeagueWish { get; set; }

        public TeamRegistration()
        {
            this.UserAuthIds = new List<int>();
        }
    }
}
