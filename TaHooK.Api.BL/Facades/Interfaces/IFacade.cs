using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Common;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IWithId
    where TDetailModel : class, IWithId
{
    Task<IEnumerable<TListModel>> GetAllAsync();
    Task<TDetailModel?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(TDetailModel model);
    Task DeleteAsync(Guid id);
}