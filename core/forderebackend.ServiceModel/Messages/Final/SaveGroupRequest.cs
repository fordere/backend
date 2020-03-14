using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/group/{GroupId}", "POST", Summary = "Save a single Group")]
    public class SaveGroupRequest : IReturnVoid
    {
        public int FinalDayCompetitionId { get; set; }

        public int GroupId { get; set; }

        public int NumberOfSuccessor { get; set; }

        public int Number { get; set; }
    }
}