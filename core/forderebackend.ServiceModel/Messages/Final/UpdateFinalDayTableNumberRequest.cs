

using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/tables/{Id}/number", "POST", Summary = "Update the number of a FinalDay table")]
    
    public class UpdateFinalDayTableNumberRequest : IReturnVoid
    {
        public int Id { get; set; }

        public int Number { get; set; }
    }
}