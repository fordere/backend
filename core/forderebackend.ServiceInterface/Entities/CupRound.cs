using System;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class CupRound : IFordereObject
    {
        [AutoIncrement] public int Id { get; set; }

        public bool IsCurrent { get; set; }

        public DateTime DeadLine { get; set; }
    }
}