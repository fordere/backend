﻿using System;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/{Id}", "PUT")]
    
    public class UpdateMatchRequest : IReturn<MatchViewDto>
    {
        public int Id { get; set; }

        public int HomeTeamScore { get; set; }

        public int GuestTeamScore { get; set; }

        public int TableId { get; set; }

        public DateTime PlayDate { get; set; }
    }
}