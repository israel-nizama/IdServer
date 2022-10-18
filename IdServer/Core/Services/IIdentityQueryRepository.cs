using IdServer.Core.QueryModel;

namespace IdServer.Core.Services
{
    public interface IIdentityQueryRepository
    {
        Task<UserIdentity> GetUserIdentity(string clientId, string email);
    }
}
