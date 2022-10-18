using IdServer.Core.CommandModel;

namespace IdServer.Core.Services
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
