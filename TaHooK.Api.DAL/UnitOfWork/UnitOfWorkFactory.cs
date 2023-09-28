using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Factories;

namespace TaHooK.Api.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<TaHooKDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public UnitOfWorkFactory(
        IDbContextFactory<TaHooKDbContext> dbContextFactory,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext(), _mapper);
}
