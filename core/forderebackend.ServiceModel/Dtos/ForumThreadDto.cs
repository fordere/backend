﻿using System;

namespace forderebackend.ServiceModel.Dtos
{
    
    public class ForumThreadDto
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string ThreadStartUser { get; set; }

        public DateTime ThreadStartDate { get; set; }

        public int NumberOfPosts { get; set; }

        public string LastActivityUser { get; set; }

        public DateTime LastActivityDate { get; set; }
    }
}