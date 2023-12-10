using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;

namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizDetail
    {
        [Parameter]
        public int? Id { get; set; } //TODO: guids

        [Inject] private NavigationManager? Navigation { get; set; }

        public QuizTemplateDetailModel Data = new()
        {
            Title = "Placeholder Quiz Template",
            Id = Guid.NewGuid(),
            Questions = {new QuestionListModel() {Id = Guid.NewGuid(), Text = "Placeholder question"}}
        };

        protected override async Task OnInitializedAsync()
        {
            //await LoadData();
            await base.OnInitializedAsync();
        }

        protected void OnLobbyCreateButton(MouseEventArgs e)
        {
            //TODO: Create a quiz game from quiz template, get its ID and navigate to the URI based on the ID
            Navigation!.NavigateTo("/lobby/DF6351D3-1093-4FD5-99CB-C050B8E0E531");
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
