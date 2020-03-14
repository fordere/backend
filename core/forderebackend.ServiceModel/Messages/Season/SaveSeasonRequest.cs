using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Season
{
    [Route("/seasons/{Id}", "POST", Summary = "Save a season")]
    public class SaveSeasonRequest : IReturn<SeasonDto>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string State { get; set; }

        public string Dates { get; set; }

        public string InfosFinalDay { get; set; }

        public string InfosPrepareSeason { get; set; }

        public string InfosEinteilung { get; set; }
    }
}