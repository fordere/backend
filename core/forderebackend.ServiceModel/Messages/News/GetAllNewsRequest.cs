using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.News
{
    [Route("/news", "GET", Summary = "Get all news.")]
    
    public class GetAllNewsRequest : PagedRequest, IReturn<List<NewsDto>>
    {
    }
}