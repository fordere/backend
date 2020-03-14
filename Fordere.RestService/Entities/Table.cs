using System.Collections.Generic;

using Fordere.ServiceInterface.Messages.Table;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    public class Table : IFordereObject
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Bar))]
        public int BarId { get; set; }

        [Reference]
        public Bar Bar { get; set; }

        [Reference]
        public List<TableAvailability> TableAvailabilities { get; set; }

        public string Name { get; set; }

        public int TableTypeValue { get; set; }

        [Ignore]
        public TableType TableType
        {
            get { return (TableType)TableTypeValue; }
            set { TableTypeValue = (int)value; }
        }
    }
}
