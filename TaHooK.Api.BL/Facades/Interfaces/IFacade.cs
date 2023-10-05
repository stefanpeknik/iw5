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
    Task<TDetailModel> SaveAsync(TDetailModel model);
    Task DeleteAsync(Guid id);
}
