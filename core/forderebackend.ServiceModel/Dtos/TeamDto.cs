namespace forderebackend.ServiceModel.Dtos
{
    public class TeamDto : NameDto
    {
        public NameDto League { get; set; }

        public NameDto Cup { get; set; }

        public NameDto HomeBar { get; set; }

        public bool IsFreeTicket { get; set; }

        public bool IsForfaitOut { get; set; }

        public int Player1Id { get; set; }

        public int Player2Id { get; set; }

        public UserDto Player1 { get; set; }

        public UserDto Player2 { get; set; }

        public string SeasonAmbition { get; set; }

        public int Group { get; set; }

        public int? BarId { get; set; }

        public QualifiedForFinalDay? QualifiedForFinalDay { get; set; }

        public string QualifiedForFinalDayComment { get; set; }

        public string WishPlayDay { get; set; }
    }
}