using ServiceStack.Auth;
using System.Collections.Generic;
using System.Linq;

namespace Fordere.ServiceInterface.Dtos
{
    public class UserMailsDto
    {
        public UserMailsDto()
        {

        }

        public UserMailsDto(IEnumerable<UserAuth> mailList)
        {
            var distinctUsers = mailList.GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToList();

            UserMails = "firstname;lastname;mail\r\n" + string.Join("\r\n", distinctUsers.Select(x => x.FirstName + ";" + x.LastName + ";" + x.Email));
        }

        public string UserMails { get; set; }
    }
}