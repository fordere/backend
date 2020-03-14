using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.User
{
    [Route("/users/{Id}/matches/current-season", "GET", Summary = "Get all matches of a user for the current season.")]
    public class UserMatchesCurrentSeasonRequest : IReturn<List<ExtendedMatchDto>>
    {
        public int Id { get; set; }
    }
}