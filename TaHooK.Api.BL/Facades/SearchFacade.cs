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

    public SearchListModel GetSearched(string query, int page, int pageSize)
    {
        var uow = _unitOfWorkFactory.Create();
        var users = uow.GetRepository<UserEntity>().Get().ProjectTo<SearchListItemModel>(_mapper.ConfigurationProvider);
        var questions = uow.GetRepository<QuestionEntity>().Get()
            .ProjectTo<SearchListItemModel>(_mapper.ConfigurationProvider);
        var answers = uow.GetRepository<AnswerEntity>().Get()
            .ProjectTo<SearchListItemModel>(_mapper.ConfigurationProvider);

        var merged = users.Concat(questions).Concat(answers);


        IEnumerable<SearchListItemModel> filtered;
        if (string.IsNullOrWhiteSpace(query))
            filtered = merged.OrderBy(x => x.Name).ToList();
        else
            filtered = merged.AsEnumerable()
                .Select(item => new
                {
                    Item = item,
                    FuzzyRatio = Fuzz.WeightedRatio(query, item.Name)
                })
                .Where(x => x.FuzzyRatio > 50) // considering fuzzy ratio
                .OrderByDescending(x => x.FuzzyRatio)
                .Select(x => x.Item).ToList();

        var items = filtered.Skip((page - 1) * pageSize).Take(pageSize);


        var result = new SearchListModel
        {
            Page = page,
            TotalItems = filtered.Count(),
            TotalPages = (int)Math.Ceiling((double)filtered.Count() / pageSize),
            Items = items
        };

        return result;
    }
}