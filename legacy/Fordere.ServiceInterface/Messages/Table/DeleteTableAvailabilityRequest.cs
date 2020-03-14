using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/tables/{TableId}/availabilities", "DELETE", Summary = "Get all tables available in a bar")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DeleteTableAvailabilityRequest : IReturnVoid
    {
        public int TableId { get; set; }

        public int TableAvailabilityId { get; set; }
    }
}