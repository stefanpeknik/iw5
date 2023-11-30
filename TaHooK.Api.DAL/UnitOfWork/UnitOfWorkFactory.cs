using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

    public IUnitOfWork Create()
    {
        return new UnitOfWork(_dbContextFactory.CreateDbContext(), _mapper);
    }
}