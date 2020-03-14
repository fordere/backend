using System.Collections.Generic;
using forderebackend.ServiceInterface.Entities.Final;
using forderebackend.ServiceModel.Dtos;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class Team : IFordereObjectWithName
    {
        public static Team CreateFreeTicket()
        {
            return new Team
            {
                BarId = 1,
                IsFreeTicket = true,
                Name = "Freilos",
                Player1Id = 1,
                Player2Id = 1
            };
        }

        [AutoIncrement] public int Id { get; set; }

        public string Name { get; set; }

        public string SeasonAmbition { get; set; }

        [References(typeof(League))] public int? LeagueId { get; set; }

        [References(typeof(Cup))] public int? CupId { get; set; }

        [References(typeof(Bar))] public int? BarId { get; set; }

        public bool IsFreeTicket { get; set; }

        public bool IsForfaitOut { get; set; }

        [References(typeof(UserAuth))] public int Player1Id { get; set; }

        [Reference] public UserAuth Player1 { get; set; }

        [References(typeof(UserAuth))] public int Player2Id { get; set; }

        [Reference] public UserAuth Player2 { get; set; }

        public int CupOrder { get; set; }

        public QualifiedForFinalDay QualifiedForFinalDay { get; set; }

        public string QualifiedForFinalDayComment { get; set; }

        public override string ToString()
        {
            return string.Format("{0} [{1}] CupOrder: {2}", Name, Id, CupOrder);
        }

        [Reference] public List<TeamInGroup> Groups { get; set; }

        public string WishPlayDay { get; set; }
    }
}