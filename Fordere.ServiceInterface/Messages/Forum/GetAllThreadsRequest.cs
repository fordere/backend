using System.Collections.Generic;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Forum
{
    [Route("/forum", "GET", Summary = "Gets all threads from the forum")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetAllThreadsRequest : IReturn<List<ForumThreadDto>>
    {
    }

    [Route("/forum/threads/{Id}", "GET", Summary = "Gets a single thread by Id")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetSingleThreadRequest : IReturn<ForumThreadDto>
    {
        public int Id { get; set; }
    }
}