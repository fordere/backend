using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/kozoom", "POST")]
    public class UpdateKozoomMatchRequest : IReturnVoid
    {
        public string key { get; set; }

        public string token { get; set; }

        public string discipline { get; set; }

        public int numberofsets { get; set; }

        public string player1id { get; set; }

        public string player2id { get; set; }

        public string player3id { get; set; }

        public string player4id { get; set; }

        public string player1name { get; set; }

        public string player2name { get; set; }

        public string player3name { get; set; }

        public string player4name { get; set; }

        public string tournamentprogress { get; set; }

        public string tablenumber { get; set; }

        public string tournamentsoftware { get; set; }

        public int diciplinetype { get; set; } = 0;
    }
}