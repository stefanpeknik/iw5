using Microsoft.AspNetCore.Components;
using TaHooK.Common.Models.Quiz;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.Quiz;

public partial class QuizDetail
{
    [Parameter]
    public Guid Id { get; set; }
    
    [Inject] private QuizFacade? Facade { get; set; }
    
    private QuizDetailModel? Data { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        Data = await Facade!.GetByIdAsync(Id);
        await base.OnInitializedAsync();
    }
}