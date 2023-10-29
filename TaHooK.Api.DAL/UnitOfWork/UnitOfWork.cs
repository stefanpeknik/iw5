using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.Repositories;

namespace TaHooK.Api.DAL.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;

    public UnitOfWork(DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper;
    }

    public IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class, IEntity
    {
        return new Repository<TEntity>(_dbContext, _mapper);
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}