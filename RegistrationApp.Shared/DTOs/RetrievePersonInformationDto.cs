namespace RegistrationApp.Shared.DTOs
{
    public class RetrievePersonInformationDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string PersonalId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int? AppartmentNumber { get; set; }
    }
}
