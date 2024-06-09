using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Mvc;



namespace RegistrationApp.BusinessLogic.Helpers
{
    public class PersonInformationHelpers
    {
        public static bool TryParseBirthDate(string birthDateString, out DateOnly birthDate)
        {
            return DateOnly.TryParseExact(birthDateString, "yyyy-MM-dd", out birthDate);
        }
    }
}
