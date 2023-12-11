using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizList
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject] private NavigationManager? Navigation { get; set; }
        [Inject] private QuizFacade? Facade { get; set; }

        List<QuizListModel>? QuizGames { get; set; }

        private bool _showPast = false;

        protected override async Task OnInitializedAsync()
        {
            QuizGames = (await Facade!.GetAllAsync()).OrderByDescending(o => o.StartedAt).ToList();
            await base.OnInitializedAsync();
        }

        protected void OnJoinQuiz(Guid quizGameId)
        {
            Navigation!.NavigateTo($"/lobby/{quizGameId}");
        }
    }
}
