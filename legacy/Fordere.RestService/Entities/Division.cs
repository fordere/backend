using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceStack.DataAnnotations;

namespace Fordere.RestService.Entities
{
    public class Division
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string PublicStripeKey { get; set; }

        public string PrivateStripeKey { get; set; }

        public string BankInformations { get; set; }

        public string Color { get; set; }

        public string Image { get; set; }

        public bool IsActive { get; set; }
    }
}
