using TaHooK.Common.Models.User;

namespace TaHooK.Web.BL.Facades;

public class UserFacade : IWebAppFacade
{
    private readonly IUserApiClient _apiClient;

    public UserFacade(IUserApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<UserListModel>> GetAllAsync()
    {
        return (await _apiClient.UsersGetAsync()).ToList();
    }

    public async Task<UserDetailModel> GetByIdAsync(Guid id)
    {
        return await _apiClient.UsersGetAsync(id);
    }
    
    public async Task<Guid> UpdateAsync(UserDetailModel model)
    {
        return (await _apiClient.UsersPutAsync(model.Id, new UserCreateUpdateModel
        {
            Name = model.Name,
            Email = model.Email,
            Photo = model.Photo
        })).Id;
    }
}