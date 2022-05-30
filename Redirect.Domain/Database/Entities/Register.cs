using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Redirect.Domain.Database.Entities
{
    public class Register
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public String Url { get; set; }

        public String Guid { get; set; }
    }
}