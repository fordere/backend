using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Season
{
    [Route("/seasons", "GET", Summary = "Get all seasons.")]
    [UsedImplicitly]
    public class GetAllSeasonsRequest : IReturn<List<SeasonDto>>
    {
    }
}