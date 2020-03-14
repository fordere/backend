namespace forderebackend.ServiceModel.Dtos
{
    public class TeamViewDto : NameDto
    {
        public int? LeagueId { get; set; }

        public int? CupId { get; set; }

        public int? BarId { get; set; }

        public bool IsFreeTicket { get; set; }

        public int Player1Id { get; set; }

        public int Player2Id { get; set; }

        public int CupOrder { get; set; }

        public string SeasonAmbition { get; set; }

        public int? CompetitionId { get; set; }

        public string CompetitionName { get; set; }

        public string Player1FirstName { get; set; }

        public string Player1LastName { get; set; }

        public string Player2FirstName { get; set; }

        public string Player2LastName { get; set; }

        public string BarName { get; set; }

        public bool IsForfaitOut { get; set; }

        public string WishPlayDay { get; set; }
    }
}