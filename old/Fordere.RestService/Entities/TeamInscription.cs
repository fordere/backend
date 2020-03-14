using ServiceStack.Auth;
using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    public class TeamInscription : IFordereObjectWithName
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Competition))]
        public int CompetitionId { get; set; }

        [References(typeof(Bar))]
        public int BarId { get; set; }

        [References(typeof(UserAuth))]
        public int Player1Id { get; set; }

        [Reference]
        public UserAuth Player1 { get; set; }

        [References(typeof(UserAuth))]
        public int Player2Id { get; set; }

        [Reference]
        public UserAuth Player2 { get; set; }

        public string SeasonAmbition { get; set; }
        public string Name { get; set; }
        public int? WishLeague { get; set; }

        public string WishPlayDay { get; set; }

        [References(typeof(League))]
        public int? AssignedLeagueId { get; set; }

        [Reference]
        public League AssignedLeague { get; set; }
    }
}