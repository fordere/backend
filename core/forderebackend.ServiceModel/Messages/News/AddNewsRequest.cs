using System;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.News
{
    [Route("/news", "POST", Summary = "Add a news.")]
    public class AddNewsRequest : IReturn<NewsDto>
    {
        public int UserAuthId { get; set; }
        public DateTime PostDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public bool IsPublished { get; set; }
        public int? DivisionId { get; set; }
    }
}