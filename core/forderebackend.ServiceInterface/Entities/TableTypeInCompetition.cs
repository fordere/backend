using Fordere.RestService.Properties;
using Fordere.ServiceInterface.Messages.Table;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    
    public class TableTypeInCompetition : IFordereObject
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Competition))]
        public int? CompetitionId { get; set; }

        [References(typeof(Competition))]
        public int? CupId { get; set; }

        public int TableTypeValue { get; set; }

        [Ignore]
        public TableType TableType
        {
            get { return (TableType)TableTypeValue; }
            set { TableTypeValue = (int)value; }
        }
    }
}