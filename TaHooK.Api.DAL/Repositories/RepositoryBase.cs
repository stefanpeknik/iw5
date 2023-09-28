using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities.Interfaces;

namespace TaHooK.Api.DAL.Repositories;

public class RepositoryBase<TEntity>: IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly DbContext _dbContext;
    
    public RepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IQueryable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(id);
        return entity;
    }
    
    public async Task<Guid> InsertAsync(TEntity entity)
    {
        var createdEntity = await _dbContext.Set<TEntity>().AddAsync(entity);
        return createdEntity.Entity.Id;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        TEntity existingEntity = await _dbContext.Set<TEntity>().SingleAsync(e => e.Id == entity.Id);
        //TODO: add mapping
        return existingEntity;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(id);

        if (entity != null)
        {
            _dbContext.Set<TEntity>().Remove(entity);    
        }
    }

}