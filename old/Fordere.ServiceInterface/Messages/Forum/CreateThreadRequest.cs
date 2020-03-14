using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Forum
{
    [Route("/forum/threads", "POST", Summary = "Create a new thread")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CreateThreadRequest : IReturn<ForumPostDto>
    {
        public string Subject { get; set; }

        public string Text { get; set; }
    }
}