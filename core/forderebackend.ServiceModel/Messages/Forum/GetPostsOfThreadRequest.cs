using System.Collections.Generic;


using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Forum
{
    [Route("/forum/{ForumThreadId}", "GET", Summary = "Gets all threads from the forum")]
    
    public class GetPostsOfThreadRequest : IReturn<List<ForumPostDto>>
    {
        public int ForumThreadId { get; set; }
    }
}