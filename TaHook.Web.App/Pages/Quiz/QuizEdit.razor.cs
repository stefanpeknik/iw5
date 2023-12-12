using System.Net.Mime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TaHooK.Common.Enums;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Answer;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.Quiz;

public class Answer
{
    public string? Text { get; set; }
    // Add image property if needed
}

public enum EntityState
{
    Unchanged,
    New,
    Updated,
    Deleted
}

public class QuestionModel
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public EntityState State { get; set; }
    public List<AnswerListModel> Answers { get; set; } = new List<AnswerListModel>();
}

public class AnswerModel
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public AnswerType Type { get; set; }
    public string Picture { get; set; }
    
    
}

public partial class QuizEdit
{
    private string QuestionText { get; set; }
    private const int MaxAnswers = 4;
    private List<Answer> Answers = new List<Answer>();
    private bool _notfirstquestion = false;
    private int _currentQuestion = 1;
    private Guid currentQuestionId { get; set; } = Guid.Empty;

    private string quizTitle { get; set; } = String.Empty;
    private ICollection<QuestionListModel> QuestionsModel { get; set; } = new List<QuestionListModel>();
    private ICollection<AnswerListModel> AnswersModel { get; set; } = new List<AnswerListModel>();

    private List<QuestionCreateUpdateModel> Questions { get; set; } = new List<QuestionCreateUpdateModel>();

    private string imageUrl { get; set; } = string.Empty;
    private bool imagePopup { get; set; } = false;
    private int currentImageAnswerIndex { get; set; } = 0;
    private List<QuestionModel> FetchQuestionsModelList { get; set; } = new List<QuestionModel>();
    private List<AnswerDetailModel> AnswersDetailModel { get; set; } = new List<AnswerDetailModel>();

    [Parameter] public Guid Id { get; set; }

    [Inject] private NavigationManager? Navigation { get; set; }
    [Inject] private QuizTemplateFacade? Facade { get; set; }

    [Inject] private QuestionFacade? QuestionFacade { get; set; }

    [Inject] private AnswerFacade? AnswerFacade { get; set; }


    private QuizTemplateDetailModel? Data { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await FetchQuestionsAsync();
    }

    protected async Task FetchQuestionsAsync()
    {
        var Data = await Facade!.GetByIdAsync(Id);
        quizTitle = Data.Title;
        var questions = Data.Questions;
        foreach (var question in questions)
        {
            Console.WriteLine($"Question: {question.Text}");
            var questionModel = new QuestionModel
            {
                Id = question.Id,
                Text = question.Text,
                State = EntityState.Unchanged,

            };
            
            Console.WriteLine($"Question: {question.Text}");
            Console.WriteLine($"QuestionId: {question.Id}");
            
            var answers = await AnswerFacade!.GetByQuestionIdAsync(question.Id);
            
            foreach (var answer in answers)
            {
                Console.WriteLine($"Answer: {answer.Text}");
                questionModel.Answers.Add(new AnswerListModel
                {
                    Id = answer.Id,
                    Text = answer.Text,
                    IsCorrect = answer.IsCorrect,
                    Type = answer.Type,
                    Picture = answer.Picture,
                    QuestionId = answer.QuestionId
                });
                
                Console.WriteLine($"AnswerId: {answer.Id}");
                Console.WriteLine($"Text: {answer.Text}");
            }
            
            FetchQuestionsModelList.Add(questionModel);
        }
    }

    protected void CreateNewEmptyQuestion()
    {

        FetchQuestionsModelList.Add(new QuestionModel
        {
            Id = Guid.NewGuid(),
            Text = String.Empty,
            State = EntityState.New,
            Answers = new List<AnswerListModel>
            {

            }
        });
    }
    
    protected void removeEmptyQuestion(int index)
    {
        if (isQuestionEmpty(index))
        {
            FetchQuestionsModelList.RemoveAt(index-1);
        }
    }

    protected bool CheckIfCreateQuestions(int newIndex)
    {
        Console.WriteLine(newIndex);
        Console.WriteLine(FetchQuestionsModelList.Count);
        if (newIndex > FetchQuestionsModelList.Count)
        {
            return true;
        }
        return false;
    }
        
    protected bool isQuestionEmpty(int index)
    {
        if (FetchQuestionsModelList.ElementAt(index-1).Text == String.Empty)
        {
            return true;
        }
        return false;
    }

    // get current question from FetchQuestionsModelList
    protected void GetCurrentQuestion(int currentQuestion)
    {
        // check if currentQuestion is in range of Questions
        if (currentQuestion < 0 || currentQuestion >= FetchQuestionsModelList.Count)
        {
            return;
        }
        //check if FetchQuestionsModelList is avaiable at currentQuestion
        if (FetchQuestionsModelList.ElementAt(currentQuestion) == null)
        {
            return;
        }
        QuestionText = FetchQuestionsModelList.ElementAt(currentQuestion).Text;
        currentQuestionId = FetchQuestionsModelList.ElementAt(currentQuestion).Id;
        InvokeAsync(StateHasChanged);
    }
    
    protected async Task UpdateQuestionUpdateModelAsync()
    {

        Console.WriteLine($"template id: {Id}");
        
        foreach (var question in FetchQuestionsModelList)
        {
            if (question.State == EntityState.New)
            {
                var questionmodel = new QuestionCreateUpdateModel
                {
                    Text = question.Text,
                    QuizTemplateId = Id,
                    Answers = new List<AnswerListModel>()
                };
                
                var id = await QuestionFacade!.CreateQuestionAsync(questionmodel);
                question.Answers.ForEach(x => x.QuestionId = id.Id);
                
                var questionCreateUpdateModel = new QuestionCreateUpdateModel
                {
                    Text = question.Text,
                    QuizTemplateId = Id,
                    Answers = question.Answers
                };
                
                await QuestionFacade.UpdateQuestionAsync(questionCreateUpdateModel, id.Id);
            }
            
            if (question.State == EntityState.Updated)
            {
                var answers = question.Answers;
            
                var questionCreateUpdateModel = new QuestionCreateUpdateModel
                {
                    Text = question.Text,
                    QuizTemplateId = Id,
                    Answers = answers
                };

                await QuestionFacade.UpdateQuestionAsync(questionCreateUpdateModel, question.Id);
            }
        }
        
    }

    private void AddAnswer()
    {
        if (FetchQuestionsModelList[_currentQuestion-1].Answers.Count() < MaxAnswers)
        {
            FetchQuestionsModelList[_currentQuestion-1].Answers.Add(new AnswerListModel
            {
                Text = "",
                IsCorrect = false,
                Type = TaHooK.Common.Enums.AnswerType.Text,
            });
        }
        InvokeAsync(StateHasChanged);
    }

    private void RemoveAnswer()
    {
        int count = FetchQuestionsModelList.ElementAt(_currentQuestion - 1).Answers.Count();
        if (count >= 0)
        {
            FetchQuestionsModelList.ElementAt(_currentQuestion-1).Answers.RemoveAt(count-1);
        } else if(FetchQuestionsModelList.ElementAt(_currentQuestion-1).Answers.Count == 1) {
            FetchQuestionsModelList.ElementAt(_currentQuestion-1).Answers.RemoveAt(0);
        }
        InvokeAsync(StateHasChanged);
    }

    private void UpdateAnswerText(int index, string text)
    {
        if (index >= 0 && index < FetchQuestionsModelList[_currentQuestion-1].Answers.Count())
        {
           
            FetchQuestionsModelList[_currentQuestion-1].Answers[index].Text = text;
            Console.WriteLine($"contentbefore: {FetchQuestionsModelList[_currentQuestion-1].Answers[index].Text}");
            StateHasChanged();
        }
    }

    private void UpdateQuestionText(string text)
    {
    FetchQuestionsModelList[_currentQuestion - 1].Text = text;
    StateHasChanged();
    }

    private void OnToggle(int index)
    {
        if (index >= 0 && index < FetchQuestionsModelList[_currentQuestion-1].Answers.Count())
        {
            FetchQuestionsModelList[_currentQuestion-1].Answers[index].IsCorrect = !FetchQuestionsModelList[_currentQuestion-1].Answers[index].IsCorrect;
            StateHasChanged();
        }
    }

    private void OnToggleQuestionVariant(int index)
    {
        if (index >= 0 && index < FetchQuestionsModelList[_currentQuestion-1].Answers.Count())
        {
            FetchQuestionsModelList[_currentQuestion-1].Answers[index].Type = FetchQuestionsModelList[_currentQuestion-1].Answers[index].Type == TaHooK.Common.Enums.AnswerType.Text ? TaHooK.Common.Enums.AnswerType.Picture : TaHooK.Common.Enums.AnswerType.Text;
            StateHasChanged();
        }
    }

    private void ImagePopup(int index)
    {
        imagePopup = true;
        
        
        currentImageAnswerIndex = index;
        
        InvokeAsync(StateHasChanged);

    }
    
    private void OnConfirmImage()
    {
        FetchQuestionsModelList[_currentQuestion-1].Answers[currentImageAnswerIndex].Picture = new Uri(imageUrl);
        imagePopup = false;
        InvokeAsync(StateHasChanged);
        imageUrl = string.Empty;
    }

    private void CreateTemplateCancel()
    {
        imagePopup = false;
    }
    
    
    private void OnNextQuestion()
    {
        _notfirstquestion = true;
        _currentQuestion++;
        Console.WriteLine($"Number of questions: {FetchQuestionsModelList.Count}");
        if (CheckIfCreateQuestions(_currentQuestion))
        {
            CreateNewEmptyQuestion();
        }
        GetCurrentQuestion(_currentQuestion);
        
        
        InvokeAsync(StateHasChanged);

    }
    
    private void OnPreviousQuestion()
    {
        _notfirstquestion = false;
        if (isQuestionEmpty(_currentQuestion))
        {
            removeEmptyQuestion(_currentQuestion);
        }

        _currentQuestion--;
        InvokeAsync(StateHasChanged);
        Console.WriteLine($"Number of questions: {FetchQuestionsModelList.Count}");
        GetCurrentQuestion(_currentQuestion);
        
        
        InvokeAsync(StateHasChanged);
    }
}