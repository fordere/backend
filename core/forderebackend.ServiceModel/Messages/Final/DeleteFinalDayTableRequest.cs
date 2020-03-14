using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/tables/{Id}", "DELETE", Summary = "Delete a single finalday table")]
    public class DeleteFinalDayTableRequest : IReturnVoid
    {
        public int Id { get; set; }
    }
}