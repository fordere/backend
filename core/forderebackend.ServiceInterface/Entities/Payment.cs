using ServiceStack.Auth;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class Payment : IFordereObject
    {
        [References(typeof(UserAuth))]
        public int UserId { get; set; }

        [Reference]
        public UserAuth User { get; set; }

        [References(typeof(Season))]
        public int SeasonId { get; set; }

        public bool HasPaid { get; set; }

        public string Comment { get; set; }

        [AutoIncrement]
        public int Id { get; set; }
    }
}
