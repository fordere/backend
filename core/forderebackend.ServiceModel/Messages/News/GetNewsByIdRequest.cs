using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.News
{
    [Route("/news/{Id}", "GET", Summary = "Get news by Id.")]
    public class GetNewsByIdRequest : IReturn<NewsDto>
    {
        public int Id { get; set; }
    }
}