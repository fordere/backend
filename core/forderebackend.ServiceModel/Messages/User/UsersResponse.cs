using System.Collections.Generic;
using forderebackend.ServiceModel.Dtos;

namespace forderebackend.ServiceModel.Messages.User
{
    public class UsersResponse : PagedResponse
    {
        public UsersResponse()
        {
            Users = new List<UserDto>();
        }

        public List<UserDto> Users { get; set; }
    }
}