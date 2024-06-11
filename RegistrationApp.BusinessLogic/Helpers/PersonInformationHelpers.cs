using RegistrationApp.Database.Repositories.Interfaces;

namespace RegistrationApp.BusinessLogic.Helpers
{
    public class PersonInformationHelpers
    {
        // Parse string => DateOnly 
        public static bool TryParseBirthDate(string birthDateString, out DateOnly birthDate)
        {
            return DateOnly.TryParseExact(birthDateString, "yyyy-MM-dd", out birthDate);
        }

        // Ensure the user owns the person
        public static async Task EnsureUserOwnsPersonAsync(IPersonRepository personRepository, Guid userId, Guid personId)
        {
            var person = await personRepository.GetPersonByIdAsync(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            if (person.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this person's information.");
            }
        }
    }
}
