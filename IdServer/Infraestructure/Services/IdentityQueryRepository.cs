using Dapper;
using IdServer.Core.QueryModel;
using IdServer.Core.Services;
using IdServer.Infraestructure.Data.Query;

namespace IdServer.Infraestructure.Services
{
    public class IdentityQueryRepository : IIdentityQueryRepository
    {
        private readonly IdServerQueryContext _context;

        public IdentityQueryRepository(IdServerQueryContext context)
        {
            _context = context;
        }

        public async Task<UserIdentity> GetUserIdentity(string clientId, string email)
        {
            var query = "SELECT a.UserId, b.UserName, b.PasswordHash, b.Email, b.FirstName, b.LastName, a.RoleCode, b.ClubCode, c.Scope" +
                " FROM ClientUserRoles a" +
                " JOIN Users b ON b.Id = a.UserId" +
                " JOIN Clients c ON c.ClientId = a.ClientId" +
                " WHERE a.ClientId = @clientId AND b.UserName = @email AND b.EmailConfirmed = '1'";

            using (var connection = _context.CreateConnection())
            {
                var userIdentity = await connection.QuerySingleOrDefaultAsync<UserIdentity>(query, new { clientId, email });

                return userIdentity;
            }
        }
    }
}
