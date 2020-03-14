using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos.FinalDay;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/tables/{Id}", "GET", Summary = "Get a single finalday table")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetFinalDayTableRequest : IReturn<FinalDayTableDto>
    {
        public int Id { get; set; }
    }
}
