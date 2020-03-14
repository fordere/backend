using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Forum
{
    [Route("/forum/posts", "POST", Summary = "Create a new post in a specific thread")]
    public class CreatePostRequest : IReturn<ForumPostDto>
    {
        public int ForumThreadId { get; set; }

        public string Text { get; set; }
    }
}