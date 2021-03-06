﻿using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Competition
{
    [Route("/competitions/{CompetitionId}/updateNotPlayed", "GET", Summary = "Sets the not played match on all not played matches in a competition")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SetMatchesNotPlayedRequest : IReturnVoid
    {
        public int CompetitionId { get; set; }
    }
}