using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizTemplateDetail
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject] private NavigationManager? Navigation { get; set; }

        [Inject] private QuizTemplateFacade? TemplateFacade { get; set; }
        [Inject] private QuizFacade? Facade { get; set; }
        [Inject] private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        public QuizTemplateDetailModel? Data { get; set; }
        
        private Guid _userId = Guid.Empty;
        private bool _showQuestions = false;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
            _userId = Guid.Parse(authState.User.Claims.First(c => c.Type.ToLower() == "id").Value);
            Data = await TemplateFacade!.GetByIdAsync(Id);
            await base.OnInitializedAsync();
        }

        protected async void OnLobbyCreateButton(MouseEventArgs e)
        {
            var gameId = await Facade!.CreateFromTemplate(Data!);
            Navigation!.NavigateTo($"/lobby/{gameId.Id}");
        }

        protected void OnEditQuizTemplate(MouseEventArgs e)
        {
            Navigation!.NavigateTo($"/quiz-edit/{Id}");
        }
    }
}
