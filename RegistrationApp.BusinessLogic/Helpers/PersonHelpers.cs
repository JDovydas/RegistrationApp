using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;


namespace RegistrationApp.BusinessLogic.Helpers
{
    public class PersonHelpers
    {
        public static bool TryParseBirthDate(string birthDateString, out DateOnly birthDate)
        {
            return DateOnly.TryParseExact(birthDateString, "yyyy-MM-dd", out birthDate);
        }

        public static async Task<String> SaveProfilePhotoAsync(IFormFile profilePhoto)
        {
            if (profilePhoto == null) return null;

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var filePath = Path.Combine(uploads, Guid.NewGuid().ToString() + Path.GetExtension(profilePhoto.FileName));
            //Add file validation e.g. GetExtension and if some files are not in expected format, than return an error?
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePhoto.CopyToAsync(stream);
            }

            using (var image = await Image.LoadAsync<Rgba32>(filePath))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(200, 200),
                    Mode = ResizeMode.Stretch
                }));
                await image.SaveAsync(filePath);
            }
            return filePath;
        }
    }
}
