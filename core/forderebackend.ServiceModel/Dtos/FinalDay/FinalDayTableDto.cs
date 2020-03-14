namespace forderebackend.ServiceModel.Dtos.FinalDay
{
    public class FinalDayTableDto : BaseDto
    {

        public int Number { get; set; }

        public string TableType { get; set; }

        public int FinalDayId { get; set; }

        public bool Disabled { get; set; }

    }
}