using ServiceStack.Auth;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities.Final
{
    public class PlayerInFinalDayCompetition
    {
        [AutoIncrement] public int Id { get; set; }

        [References(typeof(FinalDayCompetition))]
        public int FinalDayCompetitionId { get; set; }

        [References(typeof(UserAuth))] public int PlayerId { get; set; }

        [Reference] public UserAuth Player { get; set; }

        public bool IsActive { get; set; }
    }
}