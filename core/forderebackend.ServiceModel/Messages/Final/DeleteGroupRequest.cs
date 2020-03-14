using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competition/{FinalDayCompetitionId}/group/{GroupId}", "DELETE",
        Summary = "Delete a single Group")]
    public class DeleteGroupRequest : IReturnVoid
    {
        public int FinalDayCompetitionId { get; set; }

        public int GroupId { get; set; }
    }
}