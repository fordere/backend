using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.News
{
    [Route("/news", "GET", Summary = "Get all news.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllNewsRequest : PagedRequest, IReturn<List<NewsDto>>
    {
    }
}