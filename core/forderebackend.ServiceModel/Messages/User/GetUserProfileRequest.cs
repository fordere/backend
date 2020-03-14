using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.User
{
    [Route("/users/{Id}/profile", "GET", Summary = "Get the profile of a user.")]
    public class GetUserProfileRequest : IReturn<UserProfileResponse>
    {
        public int Id { get; set; }
    }

    [Route("/users/find/{Query}/{CompetitionId}", "GET", Summary = "Find users")]
    public class FindPossiblePartnersRequest : IReturn<List<UserDto>>
    {
        public int CompetitionId { get; set; }

        public string Query { get; set; }
    }
}