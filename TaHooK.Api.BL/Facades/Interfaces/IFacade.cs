using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Common;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IWithId
    where TDetailModel : class, IWithId
{
    void IncludeNavigationPathDetails(ref IQueryable<TEntity> query);
    
    Task<IEnumerable<TListModel>> GetAllAsync();
    Task<TDetailModel?> GetByIdAsync(Guid id);
    Task<Guid> CreateOrUpdate(TDetailModel model);
    Task<Guid?> CreateAsync(TDetailModel model);
    Task<Guid> UpdateAsync(TDetailModel model);
    Task DeleteAsync(Guid id);
}
