﻿using Fordere.ServiceInterface.Annotations;

namespace Fordere.ServiceInterface.Dtos
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TableEntryDto
    {
        public int Id { get; set; }

        public int LeagueId { get; set; }

        public int TeamId { get; set; }

        public TeamDto Team { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesWonExtension { get; set; }

        public int GamesDraw { get; set; }

        public int GamesLostExtension { get; set; }

        public int GamesLost { get; set; }

        public int Points { get; set; }

        public int SetsWon { get; set; }

        public int SetsLost { get; set; }

        public int Rank { get; set; }

        public int EstimatedRank { get; set; }

        public int PlusMinus { get; set; }
    }
}