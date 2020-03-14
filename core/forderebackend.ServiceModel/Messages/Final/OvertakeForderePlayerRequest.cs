using forderebackend.ServiceModel.Dtos.FinalDay;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/overtakeforderplayer", "POST",
        Summary = "Adds fordere player to Competition")]
    public class OvertakeForderePlayerRequest : IReturn<FinalDayPlayerInCompetitionDto>
    {
        public int FinalDayCompetitionId { get; set; }

        public int PlayerId { get; set; }
    }
}