using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Competition
{
    [Route("/competitions/{CompetitionId}/generate", "POST",
        Summary = "Generates all Teams & Matches for a competition. Existing Teams/Matches will be deleted.")]
    public class CreateTeamsAndMatchesRequest : IReturnVoid
    {
        public int CompetitionId { get; set; }
    }
}