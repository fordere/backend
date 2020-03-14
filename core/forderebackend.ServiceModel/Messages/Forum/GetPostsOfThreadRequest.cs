using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Forum
{
    [Route("/forum/{ForumThreadId}", "GET", Summary = "Gets all threads from the forum")]
    public class GetPostsOfThreadRequest : IReturn<List<ForumPostDto>>
    {
        public int ForumThreadId { get; set; }
    }
}