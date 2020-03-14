using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Team
{
    [Route("/teams/{Id}", "POST PATCH", Summary = "Updates a team")]
    public class UpdateTeamRequest : IReturn<TeamDto>
    {
        public int Id { get; set; }

        public int? BarId { get; set; }

        public string Name { get; set; }

        public string SeasonAmbition { get; set; }

        public bool? IsForfaitOut { get; set; }

        public int? Player1Id { get; set; }

        public int? Player2Id { get; set; }

        public string WishPlayDay { get; set; }

        public QualifiedForFinalDay? QualifiedForFinalDay { get; set; }

        public string QualifiedForFinalDayComment { get; set; }
    }
}