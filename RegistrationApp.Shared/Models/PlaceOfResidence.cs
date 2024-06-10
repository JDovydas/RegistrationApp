namespace RegistrationApp.Shared.Models
{
    public class PlaceOfResidence : CommonProperties
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int? AppartmentNumber { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
    }
}
