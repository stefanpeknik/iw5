using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizDetail
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject] private NavigationManager? Navigation { get; set; }

        [Inject] private QuizTemplateFacade? TemplateFacade { get; set; }
        [Inject] private QuizFacade? Facade { get; set; }

        public QuizTemplateDetailModel? Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await TemplateFacade!.GetByIdAsync(Id);
            await base.OnInitializedAsync();
        }

        protected async void OnLobbyCreateButton(MouseEventArgs e)
        {
            var gameId = await Facade!.CreateFromTemplate(Data!);
            Navigation!.NavigateTo($"/lobby/{gameId}");
        }

        protected void OnEditQuizTemplate(MouseEventArgs e)
        {
            //TODO: Navigate to edit view
        }

        //private async Task LoadData()
        //{
        //    //TODO: Load data from facade
        //}
    }
}
