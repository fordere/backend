﻿using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.League
{
    [Route("/leagues", "POST", Summary = "Create league")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AddLeagueRequest : IReturn<LeagueDto>
    {
        public LeagueMatchCreationMode LeagueMatchCreationMode { get; set; }
        public int Number { get; set; }
        public int Group { get; set; }
        public int CompetitionId { get; set; }
    }
}