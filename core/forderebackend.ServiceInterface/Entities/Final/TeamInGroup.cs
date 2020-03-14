using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities.Final
{
    public class TeamInGroup
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Group))]
        public int GroupId { get; set; }

        [Reference]
        public Group Group { get; set; }

        [References(typeof(Team))]
        public int TeamId { get; set; }

        [Reference]
        public Team Team { get; set; }

        public int Settlement { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + Settlement + ")";
        }
    }
}