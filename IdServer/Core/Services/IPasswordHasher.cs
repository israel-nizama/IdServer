using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace IdServer.Core.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password, KeyDerivationPrf prf, int saltSize, int numberOfIterations, int outputKeySize);
        bool VerifyHashedPassword(string hashedPassword, string providedPassword, KeyDerivationPrf prf, int saltSize, int numberOfIterations, int outputKeySize);
    }
}
