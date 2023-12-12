using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TaHooK.Common.Models.User;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.User;

public partial class UserDetail
{
    [Parameter]
    public Guid Id { get; set; }
    
    [Inject] private UserFacade? Facade { get; set; }
    [Inject] private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

    private Guid _userId = Guid.Empty;

    private string _photoUrl = String.Empty;
    
    private string _badPhotoMessage = String.Empty;
        
    private UserDetailModel? User { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
        _userId = Guid.Parse(authState.User.Claims.First(c => c.Type.ToLower() == "id").Value);
        User = await Facade!.GetByIdAsync(Id);
        _photoUrl = User.Photo.ToString();
        await base.OnInitializedAsync();
    }
    
    private async Task ChangePhoto()
    {
        if (Facade == null || User == null) return;
        try
        {
            User.Photo = new Uri(_photoUrl);
        } catch (ArgumentNullException)
        {
            _badPhotoMessage = "Please enter a URL";
            return;
        } catch (UriFormatException)
        {
            _badPhotoMessage = "Please enter a valid URL";
            return;
        }
        await Facade.UpdateAsync(User);
        await InvokeAsync(StateHasChanged);
    }
}