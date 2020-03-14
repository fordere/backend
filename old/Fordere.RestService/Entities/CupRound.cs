using System;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    public class CupRound : IFordereObject
    {
        [AutoIncrement]
        public int Id { get; set; }

        public bool IsCurrent { get; set; }

        public DateTime DeadLine { get; set; }
    }
}
