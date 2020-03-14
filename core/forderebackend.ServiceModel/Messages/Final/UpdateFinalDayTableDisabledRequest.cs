using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Final
{
    [Route("/finalday/tables/{Id}/disabled", "POST", Summary = "Update the disabled state of a FinalDay table")]
    public class UpdateFinalDayTableDisabledRequest : IReturnVoid
    {
        public int Id { get; set; }

        public bool Disabled { get; set; }
    }
}