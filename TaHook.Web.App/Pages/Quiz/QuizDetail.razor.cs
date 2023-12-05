using Microsoft.AspNetCore.Components;

namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizDetail
    {
        [Parameter]
        public int? Id { get; set; } //TODO: guid

        public QuizDetailModel data = new("Title", DateTime.Today, false);

        public record QuizDetailModel(string Title, DateTime Schedule, bool Finished);

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            //TODO: Load data from facade
        }
    }
}
