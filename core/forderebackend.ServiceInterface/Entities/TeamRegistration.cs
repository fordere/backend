using System.Collections.Generic;

namespace forderebackend.ServiceInterface.Entities
{
    public class TeamRegistration
    {
        public string Name { get; set; }
        public List<int> UserAuthIds { get; set; }
        public int? LeagueWish { get; set; }

        public TeamRegistration()
        {
            UserAuthIds = new List<int>();
        }
    }
}