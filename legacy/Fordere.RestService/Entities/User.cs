using System;
using System.Collections.Generic;

namespace Fordere.RestService.Entities
{
    public class UserContact : IFordereObjectWithName
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }

        public string Name
        {
            get { return this.FirstName + " " + this.LastName; }
            set { }
        }
    }

    public class User : UserContact
    {
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime JoinDate { get; set; }
        public bool HasPaid { get; set; }
        public List<string> Roles { get; set; }
        public bool IsDeleted { get; set; }
        public List<long> TeamIds { get; set; }

        public List<Team> Teams { get; set; } 

        /// <summary>
        /// Legacy id, will be removed
        /// </summary>
        public int DrupalId { get; set; }

        /// <summary>
        /// Legacy id, will be removed
        /// </summary>
        public int PlayerId { get; set; }

        public User()
        {
            this.Roles = new List<string>();
            this.TeamIds = new List<long>();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2}) [{3}] {4}", this.FirstName, this.LastName, this.EMail, this.Id, this.Phone);
        }
    }
}
