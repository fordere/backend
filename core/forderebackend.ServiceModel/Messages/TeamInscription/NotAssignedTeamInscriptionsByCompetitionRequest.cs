using ServiceStack;

namespace forderebackend.ServiceModel.Messages.TeamInscription
{
    [Route("/teaminscriptions/{CompetitionId}/notAssigned")]
    public class NotAssignedTeamInscriptionsByCompetitionRequest
    {
        [ApiMember(Name = "CompetitionId", Description = "Id of the Competition", ParameterType = "path",
            DataType = "int", IsRequired = true)]
        public int? CompetitionId { get; set; }
    }
}