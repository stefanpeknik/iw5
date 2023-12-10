using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.User;


namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizLobby : IAsyncDisposable
    {
        [Parameter]
        public Guid? Id { get; set; }

        public Guid User { get; set; } = Guid.Parse("A7F6F50A-3B1A-4065-8274-62EDD210CD1A"); // TODO: temp hardcoded

        public QuestionDetailModel? Question { get; set; }
        public List<UserListModel> Users { get; set; } = new ();

        [Inject] private NavigationManager? Navigation { get; set; }
        private HubConnection? _hubConnection;
        private bool _quizStarted = false;
        private bool _quizFinished = false;

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
            _hubConnection.On("NextQuestion", (QuestionDetailModel? question) => OnNextQuestion(question));
            _hubConnection.On("UsersInLobby", (IEnumerable<UserListModel> users) => OnUsersUpdate(users));


            await _hubConnection.StartAsync();
            await _hubConnection.SendAsync("JoinQuiz", Id);

            await base.OnInitializedAsync();
        }
        protected void OnNextQuestion(QuestionDetailModel? question)
        {
            if (question is null)
            {
                _quizFinished = true;
            }
            Question = question;
            InvokeAsync(StateHasChanged);
        }

        protected void OnUsersUpdate(IEnumerable<UserListModel> users)
        {
            var userList = users.ToList();
            Console.WriteLine("Loaded users");
            Console.WriteLine(userList.Count);
            Users = userList;
            InvokeAsync(StateHasChanged);
        }

        protected async Task OnStartQuizButton(MouseEventArgs e)
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.SendAsync("StartQuiz", Id);
                Console.WriteLine("Started the quiz");
                _quizStarted = true;
            }
        }

        protected void OnAnswerQuestion(AnswerListModel answer)
        {
            Console.WriteLine($"Seleceted the answer {answer.Text}");
        }
    }
}
