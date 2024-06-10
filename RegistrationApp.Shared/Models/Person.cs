using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationApp.Shared.Models
{
    public class Person : CommonProperties
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public string PersonalId { get; set; } //Standard: info in database is saved as string - no calculations are made using them
        public string PhoneNumber { get; set; } //Standard: info in database is saved as string - no calculations are made using them
        public string Email { get; set; }
        public string FilePath { get; set; }
        [ForeignKey(nameof(PlaceOfResidence))]
        public Guid PlaceOfResidenceId { get; set; }
        public PlaceOfResidence PlaceOfResidence { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
