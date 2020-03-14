using System;

namespace forderebackend.ServiceModel.Dtos
{
    public class ForumPostDto
    {
        public int Id { get; set; }

        public long UserAuthId { get; set; }

        public UserDto UserAuth { get; set; }

        public long ForumThreadId { get; set; }

        public ForumThreadDto ForumThread { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}