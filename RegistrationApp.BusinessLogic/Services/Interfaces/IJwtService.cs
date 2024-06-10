namespace RegistrationApp.BusinessLogic.Services.Interfaces
{
    public interface IJwtService
    {
        public string GetJwtToken(string userId, string role);
    }
}
