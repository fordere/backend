using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class Cup : IFordereObjectWithName
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [References(typeof(Competition))]
        public int CompetitionId { get; set; }

        [Reference]
        public Competition Competition { get; set; }

        public int CurrentRound { get; set; }
    }
}