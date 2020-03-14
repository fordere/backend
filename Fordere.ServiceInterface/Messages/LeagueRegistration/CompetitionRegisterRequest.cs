using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.LeagueRegistration
{
    [Route("/competitions/{CompetitionId}/register", "POST", Summary = "Register for a league.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CompetitionRegisterRequest : IReturnVoid
    {
        [ApiMember(Name = "CompetitionId", Description = "Id of the Competition", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int CompetitionId { get; set; }

        [ApiMember(Name = "Player1Id", Description = "Score of the Home Team", ParameterType = "model", DataType = "int", IsRequired = true)]
        public int Player1Id { get; set; }

        [ApiMember(Name = "Player2Id", Description = "Score of the Home Team", ParameterType = "model", DataType = "int", IsRequired = true)]
        public int Player2Id { get; set; }

        [ApiMember(Name = "Name", Description = "Score of the Home Team", ParameterType = "model", DataType = "string", IsRequired = true)]
        public string Name { get; set; }

        [ApiMember(Name = "WishLeague", Description = "Score of the Home Team", ParameterType = "model", DataType = "int", IsRequired = false)]
        public int? WishLeague { get; set; }

        public string WishPlayDay { get; set; }

        [ApiMember(Name = "BarId", Description = "Homelocation", ParameterType = "model", DataType = "int", IsRequired = false)]
        public int BarId { get; set; }

        [ApiMember(Name = "SeasonAmbition", Description = "Score of the Home Team", ParameterType = "model", DataType = "string", IsRequired = false)]
        public string SeasonAmbition { get; set; }
    }
}