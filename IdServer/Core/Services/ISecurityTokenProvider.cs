using IdServer.Core.QueryModel;

namespace IdServer.Core.Services
{
    public interface ISecurityTokenProvider
    {
        string GenerateAccessToken(UserIdentity userIdentity);
        string GenerateToken(int userId);
        bool ValidateToken(string token, int userId);
    }
}
