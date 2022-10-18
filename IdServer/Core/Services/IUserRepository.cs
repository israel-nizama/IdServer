using IdServer.Core.CommandModel;

namespace IdServer.Core.Services
{
    public interface IUserRepository : IRepository<User>
    {
        User Add(User user);
        Task<User> FindByPk(int id);
        Task<User> FindByEmail(string email);
    }
}
