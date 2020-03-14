using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competitions/{Id}", "DELETE", Summary = "Get's all finished FinalDayCompetitions of a season")]
    public class DeleteFinalDayCompetitionRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}