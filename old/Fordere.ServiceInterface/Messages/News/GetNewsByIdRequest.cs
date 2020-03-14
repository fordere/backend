using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.News
{
    [Route("/news/{Id}", "GET", Summary = "Get news by Id.")]
    public class GetNewsByIdRequest : IReturn<NewsDto>
    {
        public int Id { get; set; }
    }
}