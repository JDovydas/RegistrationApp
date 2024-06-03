using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Shared.Models
{
    public class User : CommonProperties
    {
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public ICollection<Person> People { get; set; }
    }
}
