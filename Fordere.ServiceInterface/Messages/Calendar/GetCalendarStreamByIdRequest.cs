using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Calendar
{
    [Route("/calendar/{Id}", "GET", Summary = "Get calendar ICS stream by Id.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetCalendarStreamByIdRequest 
    {
        public string Id { get; set; }
    }
}