using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competitions/{FinalDayCompetitionId}/newround", "POST")]
    public class CreateNewRoundRequest : IReturnVoid
    {
        public int FinalDayCompetitionId { get; set; }
    }
}