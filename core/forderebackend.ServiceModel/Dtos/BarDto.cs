namespace forderebackend.ServiceModel.Dtos
{
    public class BarDto : NameDto
    {
        public string Adresse { get; set; }
        public string Url { get; set; }
        public byte[] Image { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public override string ToString()
        {
            return $"{Name} [{Id}]";
        }
    }
}