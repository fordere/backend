using System.Collections.Generic;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    public class Bar : IFordereObjectWithName
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adresse { get; set; }
        public string Url { get; set; }
        public byte[] Image { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public int? DivisionId { get; set; }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", this.Name, this.Id);
        }

        [Reference]
        public List<Table> Tables { get; set; }
    }
}