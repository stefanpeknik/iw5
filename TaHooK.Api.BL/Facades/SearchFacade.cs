using AutoMapper;
using AutoMapper.QueryableExtensions;
using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Search;

namespace TaHooK.Api.BL.Facades;

public class SearchFacade : ISearchFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SearchFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _mapper = mapper;
    }

    public SearchListModel GetSearched(string query, int page, int pageSize)
    {
        var uow = _unitOfWorkFactory.Create();

        // prepare queries
        var users = uow.GetRepository<UserEntity>().Get()
            .Where(u => u.Name.ToLower().Contains(query));
        var questions = uow.GetRepository<QuestionEntity>().Get()
            .Where(q => q.Text.ToLower().Contains(query));
        var answers = uow.GetRepository<AnswerEntity>().Get()
            .Where(a => a.Text.ToLower().Contains(query));
        
        // count total items and pages
        var totalItems = users.Count() + questions.Count() + answers.Count();
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        
        // execute queries of limited max length
        var usersQueried = users.Take(page * pageSize)
            .ProjectTo<SearchListItemModel>(_mapper.ConfigurationProvider);
        var questionsQueried = questions.Take(page * pageSize)
            .ProjectTo<SearchListItemModel>(_mapper.ConfigurationProvider);
        var answersQueried = answers.Take(page * pageSize).Include(i => i.Question)
            .ProjectTo<SearchListItemModel>(_mapper.ConfigurationProvider);

        // combine queries and select the page items
        var items = usersQueried.Concat(questionsQueried).Concat(answersQueried)
            .Take(page * pageSize).Skip((page - 1) * pageSize).ToList();

        var result = new SearchListModel
        {
            Page = page,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Items = items
        };

        return result;
    }
}
