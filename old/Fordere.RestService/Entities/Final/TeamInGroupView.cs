namespace Fordere.RestService.Entities.Final
{
    public class TeamInGroupView
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public string TeamName { get; set; }

        public int TeamId { get; set; }

        public int Settlement { get; set; }

        public string Player1FirstName { get; set; }

        public string Player1LastName { get; set; }

        public string Player2FirstName { get; set; }

        public string Player2LastName { get; set; }
    }
}