namespace RegistrationApp.Shared.Models
{
    public class User : CommonProperties
    {
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public ICollection<Person> People { get; set; }
    }
}
