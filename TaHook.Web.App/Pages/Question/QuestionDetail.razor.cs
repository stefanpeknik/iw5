using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.Question;

public partial class QuizDetail
{
    [Parameter]
    
    public Guid Id { get; set; }
    
    // [Inject] private QuestionTemplateFacade? TemplateFacade { get; set; }
    // [Inject] private QuestionFacade? Facade { get; set; }
    //
    // public QuestionTemplateDetailModel? Data { get; set; }
}