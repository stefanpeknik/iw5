using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.Repositories;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Score;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Facades;

public class LiveQuizFacade: ILiveQuizFacade
{
    private readonly ILiveQuizStateRepository _liveQuizStateRepository;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IMapper _mapper;


    public LiveQuizFacade(IUnitOfWorkFactory unitOfWorkFactory, ILiveQuizStateRepository liveQuizStateRepository, IMapper mapper)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _liveQuizStateRepository = liveQuizStateRepository;
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
    
    public string? GetUserConnectionId(Guid userId)
    {
        return _liveQuizStateRepository.GetUserConnectionId(userId);
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
        quizState.UsersAnswers.Clear();

        if (quizState.NextQuestionIndex >= quiz?.Template.Questions.Count)
        {
            quiz.Finished = true;
            await uow.CommitAsync();
            return null;
        }
        
        var question = quiz?.Template.Questions.ElementAt(quizState.NextQuestionIndex);
        quizState.CurrentQuestionId = question?.Id ?? Guid.Empty;
        quizState.NextQuestionIndex++;
        return _mapper.Map<QuestionDetailModel>(question);
    }
    
    public void AnswerQuestion(Guid quizId, Guid userId, Guid answerId)
    {
        _liveQuizStateRepository.AnswerQuestion(quizId, userId, answerId);
    }

    public async Task<List<AnswerDistributionModel>> GetAnswerDistribution(Guid quizId)
    {
        var quizState = _liveQuizStateRepository.GetQuizState(quizId);
        var uow = _unitOfWorkFactory.Create();
        var quizRepository = uow.GetRepository<AnswerEntity>();
        var answers = await quizRepository.Get().Where(a => a.QuestionId == quizState.CurrentQuestionId).ToListAsync();
        
        var userAnswers = quizState.UsersAnswers
            .GroupBy(ua => ua.AnswerId)
            .Select(group => new { AnswerId = group.Key, Count = group.Count() })
            .ToList();
        
        var answerDistribution = answers.Select(answer => 
            new AnswerDistributionModel
            {
                Name = answer.Text,
                Count = userAnswers.FirstOrDefault(ua => ua.AnswerId == answer.Id)?.Count ?? 0
            }).ToList();

        return answerDistribution;
    }
    
    public async Task InitializeQuiz(Guid quizId)
    {
        var uow = _unitOfWorkFactory.Create();
        var quizRepository = uow.GetRepository<QuizEntity>();
        var scoreRepository = uow.GetRepository<ScoreEntity>();
        var quiz = await quizRepository.Get().Where(q => q.Id == quizId).FirstOrDefaultAsync();
        var quizState = _liveQuizStateRepository.GetQuizState(quizId);
        
        if (quiz == null)
        {
            return;
        }
        
        quiz.StartedAt = DateTime.UtcNow;

        foreach (var user in quizState.Users)
        {
            var score = new ScoreEntity
            {
                QuizId = quizId,
                UserId = user,
                Score = 0,
                Id = Guid.NewGuid()
            };
            await scoreRepository.InsertAsync(score);
        }
        
        await uow.CommitAsync();
    }
    
    public async Task<List<ScoreListModel>> CalculateResult(Guid quizId)
    {
        await using var uow = _unitOfWorkFactory.Create();
        var quizRepository = uow.GetRepository<QuizEntity>();
        var scoreRepository = uow.GetRepository<ScoreEntity>();
        var answerRepository = uow.GetRepository<AnswerEntity>();
        var quiz = await quizRepository.Get().Where(q => q.Id == quizId).FirstOrDefaultAsync();
        var quizState = _liveQuizStateRepository.GetQuizState(quizId);
        
        if (quiz == null)
        {
            return null!;
        }

        var orderedUsersAnswers = quizState.UsersAnswers.OrderBy(x => x.AnswerTime);

        int maxScore = 100;
        foreach (var userAnswer in orderedUsersAnswers)
        {
            var answer = await answerRepository.Get().Where(a => a.Id == userAnswer.AnswerId).FirstOrDefaultAsync();
            var score = await scoreRepository.Get().Where(s => s.QuizId == quizId && s.UserId == userAnswer.UserId).FirstOrDefaultAsync();
            if (score == null)
            {
                continue;
            }
            
            if (answer?.IsCorrect == true)
            {
                score.Score += maxScore;
                maxScore -= maxScore/5;
                await scoreRepository.UpdateAsync(score);
            }
        }
        await uow.CommitAsync();
        var scores = await scoreRepository.Get().Where(s => s.QuizId == quizId).Include(u => u.User).ToListAsync();
        var scoreList = _mapper.Map<List<ScoreListModel>>(scores);
        return scoreList;
    }
}