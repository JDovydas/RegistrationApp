using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Shared.DTOs
{
    public class UpdatePersonDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? BirthDate { get; set; }
        public string? PersonalId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
    }

}
