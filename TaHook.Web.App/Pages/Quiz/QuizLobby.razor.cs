using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.User;
using TaHooK.Web.BL.Facades;


namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizLobby : IAsyncDisposable
    {
        [Parameter]
        public Guid Id { get; set; }

        public Guid User { get; set; } = Guid.Parse("A7F6F50A-3B1A-4065-8274-62EDD210CD1A"); // TODO: temp hardcoded

        public QuizDetailModel? QuizModel { get; set; }
        public QuestionDetailModel? Question { get; set; }
        public List<UserListModel> Users { get; set; } = new ();
        public List<AnswerDistributionModel> Distribution { get; set; } = new();

        private int _currentQuestion = 0;
        private int _questionCount;

        [Inject] private QuizFacade? Facade { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }
        private HubConnection? _hubConnection;

        private QuizState _state = QuizState.Lobby;

        private enum QuizState
        {
            Lobby,
            Question,
            QuestionAnswered,
            QuestionResult,
            QuizResult
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            QuizModel = await Facade!.GetByIdAsync(Id);
            _questionCount = QuizModel.Questions.Count;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:7273/quizhub?userId={User}")
                .Build();

            _hubConnection.On("NextQuestion", (QuestionDetailModel? question) => OnNextQuestion(question));
            _hubConnection.On("UsersInLobby", (IEnumerable<UserListModel> users) => OnUsersUpdate(users));
            _hubConnection.On("AnswerDistribution",
                (List<AnswerDistributionModel> distribution) => OnAnswerDistribution(distribution));


            await _hubConnection.StartAsync();
            await _hubConnection.SendAsync("JoinQuiz", Id);

            await base.OnInitializedAsync();
        }
        protected void OnNextQuestion(QuestionDetailModel? question)
        {
            _state = QuizState.Question;
            if (_currentQuestion == _questionCount)
            {
                _state = QuizState.QuizResult;
            }

            _currentQuestion++;
            Question = question;
            InvokeAsync(StateHasChanged);
        }

        protected void OnUsersUpdate(IEnumerable<UserListModel> users)
        {
            Users = users.ToList();
            InvokeAsync(StateHasChanged);
        }

        protected void OnAnswerDistribution(List<AnswerDistributionModel> answerDistribution)
        {
            _state = QuizState.QuestionAnswered;
            Distribution = answerDistribution;
            UpdateChart();
            InvokeAsync(StateHasChanged);
        }

        protected async Task OnStartQuizButton(MouseEventArgs e)
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.SendAsync("StartQuiz", Id);
            }
        }

        protected async void OnAnswerQuestion(AnswerListModel answer)
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.SendAsync("AnswerQuestion", Id, answer.Id);
            }
        }
        protected async void OnGetNextQuestionButton()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.SendAsync("GetNextQuestion", Id);
            }
        }

        private PieChart? _pieChart = default;
        private PieChartOptions _pieChartOptions = default!;
        private ChartData _chartData = default!;
        protected async void UpdateChart()
        {
            var labels = new List<string>();
            var datasets = new List<IChartDataset>();
            var counts = new List<double>();
            var colors = new List<string>();
            int index = 0;
            foreach (var model in Distribution)
            {
                labels.Add(model.Name!);
                counts.Add((double)model.Count!);
                colors.Add(ColorBuilder.CategoricalTwelveColors[index]);
                index++;
            }

            var dataset = new PieChartDataset()
            {
                Data = counts,
                BackgroundColor = colors,
                BorderColor = colors,
            };
            datasets.Add(dataset);

            _chartData = new ChartData
            {
                Labels = labels,
                Datasets = datasets
            };

            _pieChartOptions = new PieChartOptions()
            {
                Responsive = true
            };

            if (_pieChart is not null)
            {
                await _pieChart.UpdateAsync(_chartData, _pieChartOptions);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_pieChart is not null)
            {
                await _pieChart.InitializeAsync(_chartData, _pieChartOptions);
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
