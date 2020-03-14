

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.TeamInscription
{
    [Route("/teaminscriptions/competition/{CompetitionId}")]
    
    public class TeamInscriptionsByCompetition
    {
        [ApiMember(Name = "CompetitionId", Description = "Id of the Competition", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int? CompetitionId { get; set; }
    }
}