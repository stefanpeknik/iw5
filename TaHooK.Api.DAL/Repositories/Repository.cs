using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities.Interfaces;

namespace TaHooK.Api.DAL.Repositories;

public class Repository<TEntity>: IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly IMapper _mapper;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(
            DbContext dbContext, 
            IMapper mapper)
    {
        _mapper = mapper;
        _dbSet = dbContext.Set<TEntity>();
    }
    
    public IQueryable<TEntity> Get() => _dbSet;

    public async ValueTask<bool> ExistsAsync(Guid id)
    {
        return id != Guid.Empty && await _dbSet.AnyAsync(e => e.Id == id);
    }

    public bool Exists(Guid id)
    {
        return id != Guid.Empty && _dbSet.Any(e => e.Id == id);
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var createdEntity = await _dbSet.AddAsync(entity);
        return createdEntity.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        TEntity existingEntity = await _dbSet.SingleAsync(e => e.Id == entity.Id);
        _mapper.Map(entity, existingEntity);
        return existingEntity;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity != null)
        {
            _dbSet.Remove(entity);    
        }
        else
        {
            throw new InvalidOperationException($"Entity with id {id} was not found");
        }
    }

}