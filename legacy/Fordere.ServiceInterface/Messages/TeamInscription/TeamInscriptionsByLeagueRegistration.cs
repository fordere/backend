using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.TeamInscription
{
    [Route("/teaminscriptions/competition/{CompetitionId}")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TeamInscriptionsByCompetition
    {
        [ApiMember(Name = "CompetitionId", Description = "Id of the Competition", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int? CompetitionId { get; set; }
    }
}