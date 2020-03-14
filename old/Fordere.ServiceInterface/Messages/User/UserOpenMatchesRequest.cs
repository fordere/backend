using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/users/{UserId}/openmatches", "GET", Summary = "Get all open matches of a user.")]
    public class UserOpenMatchesRequest : IReturn<List<MatchDto>>
    {
        public int UserId { get; set; }
    }
}