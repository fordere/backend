using System;
using System.Collections.Generic;
using forderebackend.ServiceModel.Messages.Final;
using forderebackend.ServiceModel.Messages.Table;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities.Final
{
    public class FinalDayCompetition : IFordereObjectWithName
    {
        [AutoIncrement] public int Id { get; set; }

        public string Name { get; set; }

        public TableType TableType { get; set; }

        public DateTime StateDate { get; set; }

        public CompetitionMode CompetitionMode { get; set; }

        public FinalDayCompetitionState State { get; set; }

        public int Priority { get; set; }

        [References(typeof(FinalDay))] public int FinalDayId { get; set; }

        [Reference] public FinalDay FinalDay { get; set; }

        [Reference] public List<Group> Groups { get; set; }

        [Reference] public List<PlayerInFinalDayCompetition> Players { get; set; }
    }
}