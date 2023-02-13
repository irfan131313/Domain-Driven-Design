using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain
{
    public class AppUser : IdentityUser
    {

        public int PersonelId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
    }
}
