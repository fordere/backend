using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/users/{UserId}/openmatches", "GET", Summary = "Get all open matches of a user.")]
    public class UserOpenMatchesRequest : IReturn<List<MatchDto>>
    {
        public int UserId { get; set; }
    }
}