using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.TeamInscription
{
    [Route("/teaminscriptions/{CompetitionId}/notAssigned")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class NotAssignedTeamInscriptionsByCompetition
    {
        [ApiMember(Name = "CompetitionId", Description = "Id of the Competition", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int? CompetitionId { get; set; }
    }
}