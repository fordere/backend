using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;

namespace forderebackend.ServiceModel.Messages.User
{
    public class MatchesResponse : PagedResponse
    {
        public MatchesResponse()
        {
            Matches = new List<MatchViewDto>();
        }

        public List<MatchViewDto> Matches { get; set; }
    }
}