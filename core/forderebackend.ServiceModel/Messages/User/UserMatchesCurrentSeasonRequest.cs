using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/users/{Id}/matches/current-season", "GET", Summary = "Get all matches of a user for the current season.")]
    public class UserMatchesCurrentSeasonRequest : IReturn<List<ExtendedMatchDto>>
    {
        public int Id { get; set; }
    }
}