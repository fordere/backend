

using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Calendar
{
    [Route("/calendar/{Id}", "GET", Summary = "Get calendar ICS stream by Id.")]
    
    public class GetCalendarStreamByIdRequest 
    {
        public string Id { get; set; }
    }
}