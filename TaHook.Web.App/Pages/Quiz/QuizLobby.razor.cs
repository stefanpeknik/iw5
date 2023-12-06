using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TaHooK.Common.Models;


namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizLobby : IAsyncDisposable
    {
        [Parameter]
        public Guid? Id { get; set; }

        public Guid User { get; init; } = Guid.Parse("14B1DEFA-2350-46C9-9C1C-01E4C63ACD79"); // TODO: temp hardcoded

        public QuestionDetailModel Question { get; set; }

        [Inject] private NavigationManager Navigation { get; set; }
        private HubConnection? _hubConnection;

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:7273/quizhub?userId={User}")
                .Build();

            //TODO: Events for next question, quiz start, etc..

            await _hubConnection.StartAsync();

            await base.OnInitializedAsync();
        }
    }
}
