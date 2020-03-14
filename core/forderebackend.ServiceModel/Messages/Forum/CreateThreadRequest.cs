using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Forum
{
    [Route("/forum/threads", "POST", Summary = "Create a new thread")]
    public class CreateThreadRequest : IReturn<ForumPostDto>
    {
        public string Subject { get; set; }

        public string Text { get; set; }
    }
}