using System.Collections.Generic;

namespace Fordere.ServiceInterface.Dtos
{
    public class TeamWithMatchesDto
    {
        public TeamWithMatchesDto()
        {
            this.UserIds = new List<string>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public NameDto League { get; set; }
        public NameDto Cup { get; set; }
        public NameDto HomeBar { get; set; }
        public bool IsFreeTicket { get; set; }
        public List<string> UserIds { get; set; }
        public int Group { get; set; }
        public List<MatchDto> Matches { get; set; } 
    }
}
