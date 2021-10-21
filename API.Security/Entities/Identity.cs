using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Security.Entities
{
    public class Identity
    {
        [Key]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }


    }
}
