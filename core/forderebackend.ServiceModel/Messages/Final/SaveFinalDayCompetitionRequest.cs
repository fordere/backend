using forderebackend.ServiceModel.Dtos.FinalDay;
using forderebackend.ServiceModel.Messages.Table;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competition/{Id}", "POST", Summary = "Saves a single finalday competition")]
    public class SaveFinalDayCompetitionRequest : IReturn<FinalDayCompetitionDto>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TableType TableType { get; set; }

        public string CompetitionMode { get; set; }
    }
}