using Microsoft.AspNetCore.Components;
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

        List<QuizTemplateListModel>? QuizTemplates { get; set; }

        private bool _showDeleteModal = false;
        private Guid _toDelete;

        protected override async Task OnInitializedAsync()
        {
            QuizTemplates = await Facade!.GetAllAsync();
            await base.OnInitializedAsync();
        }

        protected void OnShowDetail(Guid quizId)
        {
            Navigation!.NavigateTo($"/quiz/{quizId}");
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
            //TODO: Navigate to edit
        }
    }
}
