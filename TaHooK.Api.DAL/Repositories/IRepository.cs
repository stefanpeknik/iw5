using TaHooK.Api.DAL.Entities.Interfaces;

namespace TaHooK.Api.DAL.Repositories;

public interface IRepository<TEntity> where TEntity : IEntity
{
    IQueryable<TEntity> Get();
    ValueTask<bool> ExistsAsync(TEntity entity);
    Task<Guid> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}