using AutoMapper;
using AutoMapper.QueryableExtensions;
using FuzzySharp;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
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

    public IEnumerable<SearchListModel> GetSearched(string query, int page, int pageSize)
    {
        var uow = _unitOfWorkFactory.Create();
        var users = uow.GetRepository<UserEntity>().Get().ProjectTo<SearchListModel>(_mapper.ConfigurationProvider);
        var questions = uow.GetRepository<QuestionEntity>().Get()
            .ProjectTo<SearchListModel>(_mapper.ConfigurationProvider);
        var answers = uow.GetRepository<AnswerEntity>().Get().ProjectTo<SearchListModel>(_mapper.ConfigurationProvider);

        var merged = users.Concat(questions).Concat(answers);

        IEnumerable<SearchListModel> result;
        if (string.IsNullOrWhiteSpace(query))
            result = merged.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
        else
            result = merged.AsEnumerable()
                .Select(item => new
                {
                    Item = item,
                    FuzzyRatio = Fuzz.WeightedRatio(query, item.Name)
                })
                .Where(x => x.FuzzyRatio > 50) // considering fuzzy ratio
                .OrderByDescending(x => x.FuzzyRatio)
                .Select(x => x.Item)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

        return result;
    }
}