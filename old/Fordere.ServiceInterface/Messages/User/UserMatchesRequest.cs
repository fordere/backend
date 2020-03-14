using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/users/{UserId}/matches", "GET", Summary = "Get all matches of a user.")]
    [Route("/users/{UserId}/matches/{SeasonId}", "GET", Summary = "Get all matches of a user for a specific season.")]
    public class UserMatchesRequest : IReturn<List<ExtendedMatchDto>>
    {
        public int UserId { get; set; }
        public int? SeasonId { get; set; }
    }
}
