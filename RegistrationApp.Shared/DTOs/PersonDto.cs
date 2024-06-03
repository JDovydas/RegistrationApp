using System;
using Microsoft.AspNetCore.Http;

namespace RegistrationApp.Shared.DTOs
{

    public class PersonDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string PersonalId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public string FilePath { get; set; }
        public IFormFile ProfilePhoto { get; set; }
    }
}
