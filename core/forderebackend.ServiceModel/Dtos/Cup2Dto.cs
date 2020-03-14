﻿using System.Collections.Generic;

namespace Fordere.ServiceInterface.Dtos
{
    public class Cup2Dto : NameDto
    {
        public string SeasonId { get; set; }
        public List<NameDto> Teams { get; set; }

        public Cup2Dto()
        {
            this.Teams = new List<NameDto>();
        }
    }
}