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

        protected override async Task OnInitializedAsync()
        {
            QuizTemplates = await Facade!.GetAllAsync();
            await base.OnInitializedAsync();
        }

        protected void OnShowDetail(Guid quizId)
        {
            Navigation!.NavigateTo($"/quiz/{quizId}");
        }
    }
}
