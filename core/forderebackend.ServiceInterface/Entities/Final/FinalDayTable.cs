using forderebackend.ServiceModel.Messages.Table;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities.Final
{
    public class FinalDayTable : IFordereObject
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(FinalDay))]
        public int FinalDayId { get; set; }

        public int Number { get; set; }

        public TableType TableType { get; set; }

        public bool Disabled { get; set; }
    }
}