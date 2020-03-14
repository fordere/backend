using System;

namespace forderebackend.ServiceModel.Dtos
{
    public class ChallengeDto
    {
        public int Id { get; set; }

        public DateTime ProposedDate { get; set; }

        public DateTime AcceptedDate { get; set; }

        public NameDto ChallengingTeam { get; set; }

        public TableDto Table { get; set; }
    }
}