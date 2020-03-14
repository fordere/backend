using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Bar
{
    [Route("/bars", "GET", Summary = "Get all bars.")]
    [UsedImplicitly]
    public class GetAllBarsRequest : IReturn<List<BarDto>>
    {
    }
}