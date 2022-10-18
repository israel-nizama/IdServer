using IdServer.Core.CommandModel;
using IdServer.Core.Services;
using IdServer.Infraestructure.Data.Command;
using Microsoft.EntityFrameworkCore;

namespace IdServer.Infraestructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IdServerContext _context;

        public UserRepository(IdServerContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork { get => _context; }

        public User Add(User user)
        {
            return _context.Users.Add(user).Entity;
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == email);
        }

        public async Task<User> FindByPk(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
