using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Mvc;

namespace RegistrationApp.BusinessLogic.Helpers
{
    internal class ProfilePhotoHelpers
    {
        public static async Task<string> SaveProfilePhotoAsync(IFormFile profilePhoto)
        {
            if (profilePhoto == null) return null;

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "uploads");// Get uploads directory path.
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);// Create uploads directory if it does not exist.
            }
            // Generates unique file path for profile photo.
            var filePath = Path.Combine(uploads, Guid.NewGuid().ToString() + Path.GetExtension(profilePhoto.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create)) // Create a file stream to save profile photo.
            {
                await profilePhoto.CopyToAsync(stream); // Copy profile photo to the file stream.
            }

            // Load saved profile photo using ImageSharp library.
            using (var image = await Image.LoadAsync<Rgba32>(filePath))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(200, 200), // Resizes image to 200x200 pixels.
                    Mode = ResizeMode.Stretch //Stretches image to fit specified dimensions.
                }));

                await image.SaveAsync(filePath); // Save resized image, overwriting original file
            }
            return filePath;
        }

        public static async Task<FileContentResult> GetProfilePhotoAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath)) //Checks if file exists at the specified filePath.
            {
                {
                    throw new FileNotFoundException("Profile photo not found.");
                }
            }
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath); //reads all bytes from file at specified filePath.

            //Creates new instance of FileContentResult with byte array(fileBytes) and type ("image/jpeg").
            //FileContentResult used to return a file as the response content, which can then be used to send file data in HTTP response
            return new FileContentResult(fileBytes, "image/jpeg");
        }

        public static void DeleteProfilePhoto(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
