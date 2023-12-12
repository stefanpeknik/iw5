using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Score;
using TaHooK.Common.Models.User;
using TaHooK.Web.BL.Facades;


namespace TaHook.Web.App.Pages.Quiz
{
    public partial class QuizLobby : IAsyncDisposable
    {
        [Parameter]
        public Guid Id { get; set; }

        public QuizDetailModel? QuizModel { get; set; }
        public QuestionDetailModel? Question { get; set; }
        public List<UserListModel> Users { get; set; } = new ();
        public List<AnswerDistributionModel> Distribution { get; set; } = new();
        public List<ScoreListModel> Scores { get; set; } = new();

        private int _currentQuestion = 0;
        private int _questionCount;

        [Inject] private QuizFacade? Facade { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }
        [Inject] private IAccessTokenProvider? TokenProvider { get; set; }
        [Inject] private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
        private HubConnection? _hubConnection;

        private QuizState _state = QuizState.Lobby;
        private bool _allScores = false;
        private Guid _userId = Guid.Empty;

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
            var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
            _userId = Guid.Parse(authState.User.Claims.First(c => c.Type.ToLower() == "id").Value);
            var accessTokenResult = await TokenProvider!.RequestAccessToken();

            if (accessTokenResult.TryGetToken(out var accessToken))
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"https://localhost:7273/quizhub", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(accessToken.Value);
                    })
                    .Build();
            }
 

            _hubConnection.On("NextQuestion", (QuestionDetailModel? question) => OnNextQuestion(question));
            _hubConnection.On("UsersInLobby", (IEnumerable<UserListModel> users) => OnUsersUpdate(users));
            _hubConnection.On("AnswerDistribution",
                (List<AnswerDistributionModel> distribution) => OnAnswerDistribution(distribution));
            _hubConnection.On("QuestionResult",
                (QuestionResult result) => OnQuestionResult(result));
            _hubConnection.On("QuizResult",
                (List<ScoreListModel> scores) => OnQuizResult(scores));


            await _hubConnection.StartAsync();
            await _hubConnection.SendAsync("JoinQuiz", Id);

            await base.OnInitializedAsync();
        }

        protected async ValueTask OnQuizResult(List<ScoreListModel> scores)
        {
            Console.WriteLine("Received Quiz Results");
            _state = QuizState.QuizResult;
            Scores = scores.OrderByDescending(o => o.Score).ToList();
            await InvokeAsync(StateHasChanged);
        }

        protected void OnNextQuestion(QuestionDetailModel? question)
        {
            _state = QuizState.Question;

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
        
        protected void OnQuestionResult(QuestionResult result)
        {
            _state = QuizState.QuestionResult;
            Distribution = result.AnswerDistribution;
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

        protected async void FinishAndShowResults()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.SendAsync("QuizFinished", Id);
                Console.WriteLine("Sent Finished quiz");
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
