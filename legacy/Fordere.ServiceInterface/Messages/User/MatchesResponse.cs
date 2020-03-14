using System.Collections.Generic;

using Fordere.ServiceInterface.Dtos;

namespace Fordere.ServiceInterface.Messages.User
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