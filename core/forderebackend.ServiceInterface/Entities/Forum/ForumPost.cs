using System;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities.Forum
{
    public class ForumPost : IFordereObject
    {
        public int Id { get; set; }

        [References(typeof(UserAuth))]
        public int UserAuthId { get; set; }

        [Reference]
        public UserAuth UserAuth { get; set; }

        [References(typeof(ForumThread))]
        public int ForumThreadId { get; set; }

        [Reference]
        public ForumThread ForumThread { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}
