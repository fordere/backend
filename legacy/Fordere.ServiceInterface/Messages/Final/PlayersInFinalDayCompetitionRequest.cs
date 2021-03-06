﻿using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/competitions/{FinalDayCompetitionId}/players", "GET")]
    public class PlayersInFinalDayCompetitionRequest : IReturn<FinalDayPlayerInCompetitionDto[]>
    {
        public int FinalDayCompetitionId { get; set; }
    }

    [Route("/finalday/players/{PlayerInFinalDayCompetitionId}/toggleactive", "POST")]
    public class TogglePlayerActiveFinalDayCompetition
    {
        public int PlayerInFinalDayCompetitionId { get; set; }
    }

    [Route("/finalday/players/{PlayerInFinalDayCompetitionId}", "DELETE")]
    public class DeletePlayerInFinalDayCompetitionRequest : IReturnVoid
    {
        public int PlayerInFinalDayCompetitionId { get; set; }
    }
}