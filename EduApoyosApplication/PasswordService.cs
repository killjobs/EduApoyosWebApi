using EduApoyosApplication.Implementation;
using EduApoyosDomain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EduApoyosApplication
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<Usuario> _passwordHasher;
        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<Usuario>();
        }
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(new Usuario(), password);
        }

        public bool VerifyPassword(string hashPassword, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(new Usuario(), hashPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
