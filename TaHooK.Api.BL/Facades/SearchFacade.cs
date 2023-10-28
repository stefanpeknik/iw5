using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Search;
using AutoMapper.QueryableExtensions;
using TaHooK.Api.DAL.Entities;

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

    public static int LevenshteinDistance(string source, string target)
    {
        if (string.IsNullOrEmpty(source))
        {
            if (string.IsNullOrEmpty(target)) return 0;
            return target.Length;
        }

        if (string.IsNullOrEmpty(target)) return source.Length;

        var matrix = new int[source.Length + 1, target.Length + 1];

        // Initialize the top row and leftmost column.
        for (int i = 0; i <= source.Length; i++)
            matrix[i, 0] = i;
        for (int j = 0; j <= target.Length; j++)
            matrix[0, j] = j;

        for (int i = 1; i <= source.Length; i++)
        {
            for (int j = 1; j <= target.Length; j++)
            {
                int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);
            }
        }

        return matrix[source.Length, target.Length];
    }

    public IEnumerable<SearchListModel> GetSearched(string query, int page, int pageSize)
    {
        IUnitOfWork uow = _unitOfWorkFactory.Create();
        var users = uow.GetRepository<UserEntity>().Get().ProjectTo<SearchListModel>(_mapper.ConfigurationProvider);
        var questions = uow.GetRepository<QuestionEntity>().Get()
            .ProjectTo<SearchListModel>(_mapper.ConfigurationProvider);
        var answers = uow.GetRepository<AnswerEntity>().Get().ProjectTo<SearchListModel>(_mapper.ConfigurationProvider);

        var merged = users.Concat(questions).Concat(answers);

        var result = merged.AsEnumerable()
                                            .Select(item => new 
                                            {
                                                Item = item,
                                                Distance = LevenshteinDistance(query, item.Name),
                                                IsSubstring = item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) // considering case-insensitive match
                                            })
                                            .OrderBy(x => !x.IsSubstring) // direct substrings first
                                            .ThenBy(x => x.Distance) // then by Levenshtein distance
                                            .ThenBy(x => x.Item.Name.Length) // finally by name length
                                            .Select(x => x.Item)
                                            .Skip((page - 1) * pageSize)
                                            .Take(pageSize);

        return result;
    }

}
