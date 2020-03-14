using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Forum
{
    [Route("/forum/posts", "POST", Summary = "Create a new post in a specific thread")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CreatePostRequest : IReturn<ForumPostDto>
    {
        public int ForumThreadId { get; set; }

        public string Text { get; set; }
    }
}