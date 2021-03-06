﻿using System.Collections.Generic;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities.Forum
{
    public class ForumThread : IFordereObject
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        [Reference]
        public List<ForumPost> ForumPosts { get; set; }
    }
}