using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TaHooK.Common;

namespace TaHooK.Web.BL.Facades;

public abstract class FacadeBase<TDetailModel,TListModel>
    where TDetailModel : IWithId
{
    protected virtual string Culture => CultureInfo.DefaultThreadCurrentCulture?.Name ?? "cs";


    public virtual List<TListModel> GetAll()
    {
        var itemsAll = new List<TListModel>();
        
        return itemsAll;
    }

    public abstract Task<TDetailModel> GetByIdAsync(Guid id);

    public virtual async Task SaveAsync(TDetailModel data)
    {
        try
        {
            await SaveToApiAsync(data);
        }
        catch (HttpRequestException exception) when (exception.Message.Contains("Failed to fetch"))
        {
        }
    }

    protected abstract Task<Guid> SaveToApiAsync(TDetailModel data);

    public abstract Task DeleteAsync(Guid id);
    

}
