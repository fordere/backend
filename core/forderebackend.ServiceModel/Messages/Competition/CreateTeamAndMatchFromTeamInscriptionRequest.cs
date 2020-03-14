using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Competition
{
    [Route("/leagues/{LeagueId}/assignfromteaminscription/{TeamInscriptionId}", "POST")]
    public class CreateTeamAndMatchFromTeamInscriptionRequest : IReturnVoid
    {
        public int TeamInscriptionId { get; set; }

        public int LeagueId { get; set; }
    }
}