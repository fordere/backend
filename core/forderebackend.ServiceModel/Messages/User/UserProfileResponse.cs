using System;

namespace forderebackend.ServiceModel.Messages.User
{
    public class UserProfileResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool DivisionZuerich { get; set; }
        public bool DivisionStGallen { get; set; }
        public bool DivisionLuzern { get; set; }

        public bool DivisionWinti { get; set; }
        public string CalendarLink { get; set; }
    }
}