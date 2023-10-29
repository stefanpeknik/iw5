using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Search;
using AutoMapper.QueryableExtensions;
using TaHooK.Api.DAL.Entities;
using FuzzySharp;

namespace TaHooK.Api.BL.Facades;

public class SearchFacade : ISearchFacade
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IMapper _mapper;

    public SearchFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _mapper = mapper;
    }

    public IEnumerable<SearchListModel> GetSearched(string query, int page, int pageSize)
    {
        IUnitOfWork uow = _unitOfWorkFactory.Create();
        var users = uow.GetRepository<UserEntity>().Get().ProjectTo<SearchListModel>(_mapper.ConfigurationProvider);
        var questions = uow.GetRepository<QuestionEntity>().Get()
            .ProjectTo<SearchListModel>(_mapper.ConfigurationProvider);
        var answers = uow.GetRepository<AnswerEntity>().Get().ProjectTo<SearchListModel>(_mapper.ConfigurationProvider);

        var merged = users.Concat(questions).Concat(answers);

        IEnumerable<SearchListModel> result;
        if (string.IsNullOrWhiteSpace(query))
        {
            result = merged.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
        }
        else
        {
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
        }
        
        return result;
    }

}
