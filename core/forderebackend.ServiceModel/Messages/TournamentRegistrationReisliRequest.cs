using ServiceStack;

namespace forderebackend.ServiceModel.Messages
{
    [Route("/tournamentregistration/", "POST")]
    public class TournamentRegistrationRequest
    {
        public string Tournament { get; set; }

        public string Comment { get; set; }

        public string TeamName { get; set; }

        public int Total { get; set; }

        public SffPlayerDto Player1 { get; set; }

        public SffPlayerDto Player2 { get; set; }

        public SffPlayerDto Player3 { get; set; }

        public SffPlayerDto Player4 { get; set; }
    }
}