using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.Repositories;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Facades;

public class LiveQuizFacade: ILiveQuizFacade
{
    private readonly ILiveQuizStateRepository _liveQuizStateRepository;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IMapper _mapper;
    private readonly IQuizFacade _quizFacade;


    public LiveQuizFacade(IUnitOfWorkFactory unitOfWorkFactory, ILiveQuizStateRepository liveQuizStateRepository, IMapper mapper, IQuizFacade quizFacade)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _liveQuizStateRepository = liveQuizStateRepository;
        _quizFacade = quizFacade;
        _mapper = mapper;
    }
    
    public void AddUserConnection(string connectionId, Guid userId)
    {
        _liveQuizStateRepository.AddUserConnection(connectionId, userId);
    }
    
    public Guid GetUserConnection(string connectionId)
    {
        return _liveQuizStateRepository.GetUserConnection(connectionId);
    }
    
    public void RemoveUserConnection(string connectionId)
    {
        var userId = _liveQuizStateRepository.GetUserConnection(connectionId);
        _liveQuizStateRepository.RemoveUserConnection(connectionId);
    }
    
    public Guid? GetUserQuiz(Guid userId)
    {
        return _liveQuizStateRepository.GetUserQuiz(userId);
    }
    
    public void AddUserToQuiz(Guid quizId, Guid userId)
    {
        _liveQuizStateRepository.AddUserToQuiz(quizId, userId);
    }
    
    public IEnumerable<UserListModel> GetQuizUsers(Guid quizId)
    {
        var quizUsersIds = _liveQuizStateRepository.GetQuizUsers(quizId);
        var uow = _unitOfWorkFactory.Create();
        var userRepository = uow.GetRepository<UserEntity>();

        var users = userRepository.Get().Where(u => quizUsersIds.Contains(u.Id));
        var quizUsers = _mapper.Map<IEnumerable<UserListModel>>(users);
        
        
        return quizUsers;
    }
    
    public void RemoveUserFromQuiz(Guid quizId, Guid userId)
    {
        _liveQuizStateRepository.RemoveUserFromQuiz(quizId, userId);
    }
    
    public async Task<QuestionDetailModel?> GetNextQuestion(Guid quizId)
    {

        var uow = _unitOfWorkFactory.Create();
        var quizRepository = uow.GetRepository<QuizEntity>();
        var quiz = await quizRepository.Get().Where(q => q.Id == quizId)
            .Include(q => q.Template)
            .ThenInclude(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync();
        var quizState = _liveQuizStateRepository.GetQuizState(quizId);

        if (quizState.CurrentQuestionIndex >= quiz?.Template.Questions.Count)
        {
            return null;
        }
        
        var question = quiz?.Template.Questions.ElementAt(quizState.CurrentQuestionIndex);
        quizState.CurrentQuestionIndex++;
        return _mapper.Map<QuestionDetailModel>(question);
    } 
}