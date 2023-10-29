using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.Repositories;
using TaHooK.Common;
using AutoMapper;
using TaHooK.Api.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Collections;
using TaHooK.Common.Models;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Api.BL.Facades;

public abstract class CrudFacadeBase<TEntity, TListModel, TDetailModel, TCreateUpdateModel> : ICrudFacade<TEntity, TListModel, TDetailModel, TCreateUpdateModel>
where TEntity : class, IEntity
where TListModel : IWithId
where TDetailModel : class, IWithId
where TCreateUpdateModel : class
{
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory;
    protected readonly IMapper Mapper;

    protected CrudFacadeBase(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
    {
        UnitOfWorkFactory = unitOfWorkFactory;
        Mapper = mapper;
    }

    public virtual List<string> NavigationPathDetails => new();

    public void IncludeNavigationPathDetails(ref IQueryable<TEntity> query)
    {
        foreach (var navigationPathDetail in NavigationPathDetails)
        {
            query = string.IsNullOrWhiteSpace(navigationPathDetail)
                ? query
                : query.Include(navigationPathDetail);
        }
    }

    public virtual async Task<IEnumerable<TListModel>> GetAllAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query = uow.GetRepository<TEntity>().Get();

        IncludeNavigationPathDetails(ref query);

        List<TEntity> entities = await query.ToListAsync();
        
        return Mapper.Map<IEnumerable<TListModel>>(entities);
    }

    public virtual async Task<TDetailModel?> GetByIdAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
        IQueryable<TEntity> query = uow.GetRepository<TEntity>().Get();

        IncludeNavigationPathDetails(ref query);
        
        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        
        return entity == null ? null : Mapper.Map<TDetailModel>(entity);
    }
    
    public async Task<IdModel> CreateAsync(TCreateUpdateModel model)
    {
        var entity = Mapper.Map<TEntity>(model);

        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<TEntity>();

        entity.Id = Guid.NewGuid();
        var createdEntity = await repository.InsertAsync(entity);
        
        await uow.CommitAsync();

        var result = Mapper.Map<IdModel>(createdEntity);
        return result;
    }

    public async Task<IdModel> UpdateAsync(TCreateUpdateModel model, Guid id)
    {
        var entity = Mapper.Map<TEntity>(model);
        
        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<TEntity>();
        
        entity.Id = id;
        var updatedEntity = await repository.UpdateAsync(entity);
        
        await uow.CommitAsync();
        
        var result = Mapper.Map<IdModel>(updatedEntity);
        return result;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
        IRepository<TEntity> repository = uow.GetRepository<TEntity>();
        
        await repository.DeleteAsync(id);
        
        await uow.CommitAsync();
    }
}