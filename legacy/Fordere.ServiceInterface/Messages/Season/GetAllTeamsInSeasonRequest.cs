﻿using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Season
{
    [Route("/seasons/{SeasonId}/teams", "GET", Summary = "Get all teams in a season")]
    [UsedImplicitly]
    public class GetAllTeamsInSeasonRequest : IReturn<List<TeamDto>>
    {
        public int SeasonId { get; set; }   
    }
}