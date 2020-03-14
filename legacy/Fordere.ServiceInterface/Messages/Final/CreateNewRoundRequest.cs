using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competitions/{FinalDayCompetitionId}/newround", "POST")]
    public class CreateNewRoundRequest : IReturnVoid
    {
        public int FinalDayCompetitionId { get; set; }
    }
}