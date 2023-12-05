using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.Repositories;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Facades;

public class QuizGameManager: IQuizGameManager
{
    private readonly IQuizGameStateRepository _quizGameStateRepository;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IMapper _mapper;


    public QuizGameManager(IUnitOfWorkFactory unitOfWorkFactory, IQuizGameStateRepository quizGameStateRepository, IMapper mapper)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _quizGameStateRepository = quizGameStateRepository;
        _mapper = mapper;
    }
    
    public void AddUserToQuiz(Guid quizId, Guid userId)
    {
        _quizGameStateRepository.AddUserToQuiz(quizId, userId);
    }
    
    public IEnumerable<UserListModel> GetQuizUsers(Guid quizId)
    {
        var quizUsersIds = _quizGameStateRepository.GetQuizUsers(quizId);
        var uow = _unitOfWorkFactory.Create();
        var userRepository = uow.GetRepository<UserEntity>();

        var users = userRepository.Get().Where(u => quizUsersIds.Contains(u.Id));
        var quizUsers = _mapper.Map<IEnumerable<UserListModel>>(users);
        
        
        return quizUsers;
    }
}