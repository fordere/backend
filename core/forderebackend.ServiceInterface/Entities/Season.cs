using System.Collections.Generic;

using Fordere.ServiceInterface.Messages.Season;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    public class Season : IFordereObjectWithName
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public SeasonState State { get; set; }

        [Reference]
        public List<Competition> Competitions { get; set; }

        [Reference]
        public Final.FinalDay FinalDay { get; set; }

        public int DivisionId { get; set; }

        public string Dates { get; set; }

        public string InfosFinalDay { get; set; }

        public string InfosPrepareSeason { get; set; }

        public string InfosEinteilung { get; set; }
    }
}
