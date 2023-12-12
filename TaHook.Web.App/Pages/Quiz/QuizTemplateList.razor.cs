using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizTemplateList
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject] private NavigationManager? Navigation { get; set; }
        [Inject] private QuizTemplateFacade? Facade { get; set; }
        [Inject] private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        List<QuizTemplateListModel>? QuizTemplates { get; set; }

        private bool _showDeleteModal = false;
        private bool _showCreateTemplate = false;
        private Guid _userId = Guid.Empty;
        private string templateTitle { get; set; } = String.Empty;
        private Guid _toDelete;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
            _userId = Guid.Parse(authState.User.Claims.First(c => c.Type.ToLower() == "id").Value);
            Console.WriteLine(_userId);
            QuizTemplates = await Facade!.GetAllAsync();
            await base.OnInitializedAsync();
        }

        protected void OnShowDetail(Guid quizId)
        {
            Navigation!.NavigateTo($"/quiz-template/{quizId}");
        }

        protected void OnDeleteTemplate(Guid quizId)
        {
            _toDelete = quizId;
            _showDeleteModal = true;
        }

        protected async void DeleteConfirm()
        {
            await Facade!.DeleteById(_toDelete);
            _toDelete = Guid.Empty;
            _showDeleteModal = false;
            QuizTemplates = await Facade!.GetAllAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected async void DeleteCancel()
        {
            _toDelete = Guid.Empty;
            _showDeleteModal = false;
            Console.WriteLine(_showDeleteModal);
            await InvokeAsync(StateHasChanged);
        }

        protected void OnEditTemplate(Guid quizId)
        {
            Navigation!.NavigateTo($"/quiz-edit/{quizId}");
        }
        
        protected void OnCreateTemplateButton(MouseEventArgs e)
        {
            _showCreateTemplate = true;
        }
        
        protected void CreateTemplateCancel()
        {
            _showCreateTemplate = false;
        }

        protected async void OnConfirmCreateTemplate()
        {
            if (string.IsNullOrEmpty(templateTitle))
            {
                return;
            }
            _showCreateTemplate = false;
            await Facade!.CreateTemplateAsync(templateTitle);
            Console.WriteLine("Create");
            QuizTemplates = await Facade!.GetAllAsync();
            InvokeAsync(StateHasChanged);
            templateTitle = string.Empty;

        }
    }
}
