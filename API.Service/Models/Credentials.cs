using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Service.Models
{
    public class Credentials
    {
        public class JWT
        {
            public string Username { get; set; }
            public string Password { get; set; }

        }
        public class Plain
        {
            public string Username { get; set; }
            public string Password { get; set; }

        }
    }
}