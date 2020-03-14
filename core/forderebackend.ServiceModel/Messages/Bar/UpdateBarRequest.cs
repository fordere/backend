using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Bar
{
    [Route("/bars/{Id}", "POST", Summary = "Update a Bar")]
    public class UpdateBarRequest : IReturn<BarDto>
    {
        public int Id { get; set; }
        public string Adresse { get; set; }
        public string Url { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Name { get; set; }
    }
}