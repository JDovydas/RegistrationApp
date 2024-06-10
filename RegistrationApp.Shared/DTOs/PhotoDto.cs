using Microsoft.AspNetCore.Http;

namespace RegistrationApp.Shared.DTOs
{
    public class PhotoDto
    {
        public IFormFile ProfilePhoto { get; set; }
    }
}
