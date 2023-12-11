using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.Quiz;

public class Answer
{
    public string? Text { get; set; }
    // Add image property if needed
}

public partial class QuizEdit
{
    private string QuestionText { get; set; }
    private const int MaxAnswers = 4;
    private List<Answer> Answers = new List<Answer>();
    private bool _notfirstquestion = false;
    private int _currentQuestion = 1;
    
    private string quizTitle { get; set; } = String.Empty;
    private ICollection<QuestionListModel> Questions { get; set; } = new List<QuestionListModel>();

    [Parameter]
    public Guid Id { get; set; }

    [Inject] private NavigationManager? Navigation { get; set; }
    [Inject] private QuizTemplateFacade? Facade { get; set; }


    private QuizTemplateDetailModel? Data { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        Data = await Facade!.GetByIdAsync(Id);
        quizTitle = Data.Title;
        await base.OnInitializedAsync();
        Questions = Data.Questions;
    }
    
    private void AddAnswer()
    {
        if (Answers.Count < MaxAnswers)
        {
            Answers.Add(new Answer());
        }
    }

    private void RemoveAnswer()
    {
        if (Answers.Count > 0)
        {
            Answers.RemoveAt(Answers.Count - 1);
        }
    }

    private void UpdateAnswerText(int index, string text)
    {
        if (index >= 0 && index < Answers.Count)
        {
            Console.WriteLine($"contentbefore: {Answers[index].Text}");
            Answers[index].Text = text;
            StateHasChanged();
        }
    }

    private void SaveAnswer(int index)
    {
        // Logic to save the answer
        // This could involve updating the backend or just confirming the change locally
        Console.WriteLine($"Answer {index}");
        // output all answers
        int i = 0;
        foreach (var answer in Answers)
        {

            Console.WriteLine($"Answer {i}: {answer.Text}");
            i++;
        }
    }

    private void UpdateQuestionText(string text)
    {
        QuestionText = text;
    }

    private void SaveQuestion()
    {
        // Logic to save the question
        // This could involve updating the backend or just confirming the change locally
        Console.WriteLine($"Question saved: {QuestionText}");
    }
    
    private void OnNextQuestion()
    {
        _notfirstquestion = true;
        InvokeAsync(StateHasChanged);
        _currentQuestion++;
    }
    
    private void OnPreviousQuestion()
    {
        _notfirstquestion = false;
        InvokeAsync(StateHasChanged);
        _currentQuestion--;
    }
}