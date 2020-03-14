using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.LeagueRegistration
{
    [Route("/teaminscriptions/{Id}/updateassignedleague", "POST", Summary = "Saves a single league registration")]
    
    public class UpdateAssignedLeagueRequest : IReturnVoid
    {
        public int Id { get; set; }

        public int? AssignedLeagueId { get; set; }
    }

    [Route("/teaminscriptions", "POST", Summary = "Create a new Teaminscription")]
    public class CreateTeamInscriptionRequest : IReturn<TeamInscriptionDto>
    {
        public string Name { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public int BarId { get; set; }
        public string SeasonAmbition { get; set; }
        public int CompetitionId { get; set; }
    }

    [Route("/teaminscriptions/{Id}", "GET", Summary = "get a teaminscription")]
    public class GetTeamInscriptionByIdRequest : IReturn<TeamInscriptionDto>
    {
        public int Id { get; set; }
    }

    [Route("/teaminscriptions/{Id}", "PUT", Summary = "Update a teaminscription")]
    public class UpdateTeamInscriptionRequest : IReturn<TeamInscriptionDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public int BarId { get; set; }
        public string SeasonAmbition { get; set; }
    }

    [Route("/season/einteilungen/{CompetitionId}", "GET")]
    
    public class EinteilungenRequest : IReturn<EinteilungLeagueDto>
    {
        public int CompetitionId { get; set; }
    }

    public class EinteilungLeagueDto
    {
        public int LeagueNumber { get; set; }

        public int LeagueGroup { get; set; }

        public List<EinteilungDto> Einteilungen { get; set; }
    }

    public class EinteilungDto
    {
        public string Team { get; set; }

        public UserDto Player1 { get; set; }

        public UserDto Player2 { get; set; }
    }
}