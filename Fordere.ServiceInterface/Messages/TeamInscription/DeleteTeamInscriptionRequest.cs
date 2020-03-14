using ServiceStack;

namespace Fordere.ServiceInterface.Messages.TeamInscription
{
    [Route("/teaminscriptions/{TeamInscriptionId}", "DELETE", Summary = "Delete a teaminscription.")]
    public class DeleteTeamInscriptionRequest
    {
        public int TeamInscriptionId { get; set; }
    }
}