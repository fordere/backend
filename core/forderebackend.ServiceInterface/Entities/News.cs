using System;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;

namespace forderebackend.ServiceInterface.Entities
{
    public class News : IFordereObject
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(UserAuth))]
        public int UserAuthId { get; set; }

        public DateTime PostDate { get; set; }

        public string Title { get; set; }

        [CustomField("MEDIUMTEXT")]
        public string Summary { get; set; }

        [CustomField("MEDIUMTEXT")]
        public string Content { get; set; }

        public bool IsPublished { get; set; }

        public int? DivisionId { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} [{2}]", this.PostDate.ToString("d"), this.Title, this.Id);
        }
    }
}
