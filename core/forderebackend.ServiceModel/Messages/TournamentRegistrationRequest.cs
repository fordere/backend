using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages
{
    [Route("/tournamentregistration/sts", "POST")]
    public class TournamentRegistrationStsRequest
    {
        public string UserMail { get; set; }

        public string UserName { get; set; }

        public List<TournamentRegistrationDto> Tournaments { get; set; }
    }

    [Route("/tournamentregistration/reisli/", "POST")]
    public class TournamentRegistrationReisliRequest
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

    public class SffPlayerDto
    {
        public string Name { get; set; }

        public string Mail { get; set; }

        public string Abo { get; set; }
    }

}
