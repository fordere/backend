using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.News
{
    [Route("/news", "GET", Summary = "Get all news.")]
    
    public class GetAllNewsRequest : PagedRequest, IReturn<List<NewsDto>>
    {
    }
}