using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizDetail
    {
        [Parameter]
        public int? Id { get; set; } //TODO: guids

        [Inject] private NavigationManager Navigation { get; set; }

        public QuizDetailModel data = new("Title", DateTime.Today, false);

        public record QuizDetailModel(string Title, DateTime Schedule, bool Finished);

        protected override async Task OnInitializedAsync()
        {
            //await LoadData();
            await base.OnInitializedAsync();
        }

        protected void OnLobbyCreateButton(MouseEventArgs e)
        {
            //TODO: Create a quiz game from quiz template, get its ID and navigate to the URI based on the ID
            Navigation.NavigateTo("/lobby/DF6351D3-1093-4FD5-99CB-C050B8E0E531");
        }

        //private async Task LoadData()
        //{
        //    //TODO: Load data from facade
        //}
    }
}
