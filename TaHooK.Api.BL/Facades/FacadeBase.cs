using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.Repositories;
using TaHooK.Common;
using AutoMapper;
using TaHooK.Api.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Collections;

namespace TaHooK.Api.BL.Facades;

public abstract class FacadeBase<TEntity, TListModel, TDetailModel> : IFacade<TEntity, TListModel, TDetailModel>
where TEntity : class, IEntity
where TListModel : IWithId
where TDetailModel : class, IWithId
{
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory;
    protected readonly IMapper Mapper;

    protected FacadeBase(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
    {
        UnitOfWorkFactory = unitOfWorkFactory;
        Mapper = mapper;
    }

    public virtual List<string> NavigationPathDetails => new();

    public IQueryable IncludeNavigationPathDetails(IQueryable<TEntity> query)
    {
        foreach (var navigationPathDetail in NavigationPathDetails)
        {
            query = string.IsNullOrWhiteSpace(navigationPathDetail)
                ? query
                : query.Include(navigationPathDetail);
        }

        return query;
    }

    public virtual async Task<IEnumerable<TListModel>> GetAllAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query = uow.GetRepository<TEntity>().Get();
        
        List<TEntity> entities = await query.ToListAsync();
        
        return Mapper.Map<IEnumerable<TListModel>>(entities);
    }

    public virtual async Task<TDetailModel?> GetByIdAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
        IQueryable<TEntity> query = uow.GetRepository<TEntity>().Get();
        
        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        
        return entity == null ? null : Mapper.Map<TDetailModel>(entity);
    }

    public virtual async Task<Guid> CreateAsync(TDetailModel model)
    {
        GuardCollectionsAreNotSet(model);
        
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
        IRepository<TEntity> repository = uow.GetRepository<TEntity>();
        
        // TODO: check if entity already exists
        
        TEntity entity = Mapper.Map<TEntity>(model);
        
        entity.Id = Guid.NewGuid();
        
        Guid insertedEntity = await repository.InsertAsync(entity);
        
        await uow.CommitAsync();
        
        return insertedEntity;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
        IRepository<TEntity> repository = uow.GetRepository<TEntity>();
        
        await repository.DeleteAsync(id);
        
        await uow.CommitAsync();
    }
    
    /// <summary>
    /// This Guard ensures that there is a clear understanding of current infrastructure limitations.
    /// This version of BL/DAL infrastructure does not support insertion or update of adjacent entities.
    /// WARN: Does not guard navigation properties.
    /// </summary>
    /// <param name="model">Model to be inserted or updated</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }
}