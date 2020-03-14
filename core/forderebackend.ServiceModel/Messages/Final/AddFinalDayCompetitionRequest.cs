using forderebackend.ServiceModel.Dtos.FinalDay;
using forderebackend.ServiceModel.Messages.Table;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/competitions", "POST", Summary = "Add a FinalDay Competition")]
    public class AddFinalDayCompetitionRequest : IReturn<FinalDayCompetitionDto>
    {
        public int FinalDayId { get; set; }

		public string Name { get; set; }

        public TableType TableType { get; set; }
        
        public CompetitionMode CompetitionMode { get; set; }

        public int Priority { get; set; }
    }
}