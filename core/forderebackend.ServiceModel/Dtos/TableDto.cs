namespace forderebackend.ServiceModel.Dtos
{
    public class TableDto : NameDto
    {
        public string BarName { get; set; }

        // TODO SSH This should be an enum -> TableType
        public string TableType { get; set; }
    }
}