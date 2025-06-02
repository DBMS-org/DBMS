using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class PasswordService
    {
        public string HashPassword(string password)
        {
            // Using BCrypt for password hashing - this is more secure than SHA256
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        
        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
} 