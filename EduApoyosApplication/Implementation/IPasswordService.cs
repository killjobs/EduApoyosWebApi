namespace EduApoyosApplication.Implementation
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashPassword, string password);
    }
}
