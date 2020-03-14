using Fordere.ServiceInterface.Dtos;

namespace Fordere.ServiceInterface.Messages.User
{
    using System.Collections.Generic;

    public class UsersResponse : PagedResponse
    {
        public UsersResponse()
        {
            this.Users = new List<UserDto>();
        }

        public List<UserDto> Users { get; set; } 
    }
}