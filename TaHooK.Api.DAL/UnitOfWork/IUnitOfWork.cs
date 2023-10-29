using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.Repositories;

namespace TaHooK.Api.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class, IEntity;

    Task CommitAsync();
}