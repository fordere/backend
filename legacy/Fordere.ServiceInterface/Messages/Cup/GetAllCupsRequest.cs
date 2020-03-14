using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Cup
{
    [Route("/cups", "GET", Summary = "Get all cups.")]
    public class GetAllCupsRequest : IReturn<List<Cup2Dto>>
    {
    }
}