

using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competitions/{Id}/state", "POST", Summary = "Update the state of a FinalDay Competition")]
    
    public class UpdateFinalDayCompetitionStateRequest : IReturnVoid
    {
        public int Id { get; set; }

        public FinalDayCompetitionState CompetitionState { get; set; }
    }
}