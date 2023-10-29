using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Common;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface ICrudFacade<TEntity, TListModel, TDetailModel> : IFacade
    where TEntity : class, IEntity
    where TListModel : IWithId
    where TDetailModel : class, IWithId
{
    void IncludeNavigationPathDetails(ref IQueryable<TEntity> query);
    
    Task<IEnumerable<TListModel>> GetAllAsync();
    Task<TDetailModel?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(TDetailModel model);
    Task<Guid> UpdateAsync(TDetailModel model);
    Task DeleteAsync(Guid id);
}
