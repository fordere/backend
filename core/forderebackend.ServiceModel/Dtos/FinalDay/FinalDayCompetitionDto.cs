using System;
using System.Collections.Generic;
using forderebackend.ServiceModel.Messages.Final;
using forderebackend.ServiceModel.Messages.Table;

namespace forderebackend.ServiceModel.Dtos.FinalDay
{
    public class FinalDayCompetitionDto : NameDto
    {
        public TableType TableType { get; set; }

        public bool OnHold { get; set; }

        public DateTime HoldDate { get; set; }

        public CompetitionMode CompetitionMode { get; set; }

        public int NumberOfSuccessor { get; set; }

        public List<GroupDto> Groups { get; set; }

        public DateTime StateDate { get; set; }

        public FinalDayCompetitionState State { get; set; }
    }
}
