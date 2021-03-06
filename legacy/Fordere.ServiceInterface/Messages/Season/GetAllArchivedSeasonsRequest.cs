﻿using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Season
{
    [Route("/seasons/archived", "GET", Summary = "Get all seasons.")]
    [UsedImplicitly]
    public class GetAllArchivedSeasonsRequest : IReturn<List<SeasonDto>>
    {
    }
}