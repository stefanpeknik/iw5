using TaHooK.Api.BL.Facades;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Repositories;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Facades;

public class QuizGameManager: IQuizGameManager
{
    private readonly UserFacade _userFacade;
    private readonly IQuizGameStateRepository _quizGameStateRepository;


    public QuizGameManager(UserFacade userFacade, IQuizGameStateRepository quizGameStateRepository)
    {
        _userFacade = userFacade;
        _quizGameStateRepository = quizGameStateRepository;
    }
    
    public void AddUserToQuiz(Guid quizId, Guid userId)
    {
        _quizGameStateRepository.AddUserToQuiz(quizId, userId);
    }
    
    public async Task<IEnumerable<UserListModel>> GetQuizUsers(Guid quizId)
    {
        var quizUsersIds = _quizGameStateRepository.GetQuizUsers(quizId);

        var users = await _userFacade.GetAllAsync();
        var quizUsers = users.Where(u => quizUsersIds.Contains(u.Id));
        
        return quizUsers;
    }
}