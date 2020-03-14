using System.Collections.Generic;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities.Final
{
    public class FinalDay : IFordereObjectWithName
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [References(typeof(Season))]
        public int SeasonId { get; set; }

        [Reference]
        public List<FinalDayTable> Tables { get; set; }
    }
}