using System;
using System.Collections.Generic;

namespace forderebackend.ServiceInterface.Entities
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
            get => FirstName + " " + LastName;
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
            Roles = new List<string>();
            TeamIds = new List<long>();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2}) [{3}] {4}", FirstName, LastName, EMail, Id, Phone);
        }
    }
}