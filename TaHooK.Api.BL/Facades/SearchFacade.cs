using System.Collections.ObjectModel;
using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Search;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

    private async Task<IEnumerable<SearchListModel>> Search<T>(string searchQuery, string fieldName) where T : class, IEntity
    {
        var propertyInfo = typeof(T).GetProperty(fieldName);
        if (propertyInfo == null) throw new ArgumentException("Field name not found on entity type.");

        var uwo = _unitOfWorkFactory.Create();

        var query = uwo.GetRepository<T>().Get();

        List<T> entities = await query.ToListAsync();
        
        
        return query
            .Select(entity => new
            {
                Entity = entity,
                Distance = LevenshteinDistance(searchQuery, propertyInfo.GetValue(entity).ToString() ?? string.Empty)
            })
            .Where(x => x.Distance < searchQuery.Length) // adjust threshold as needed
            .OrderBy(x => x.Distance)
            .Select(x => _mapper.Map<SearchListModel>(x.Entity));

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
                matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
            }
        }

        return matrix[source.Length, target.Length];
    }

    public Task<IEnumerable<SearchListModel>> GetSearchedAsync(string query, int page, int pageSize)
    {
        var users = Search<UserEntity>(query, "Name");
        var questions = Search<QuestionEntity>(query, "Text");
        var answers = Search<AnswerEntity>(query, "Text");


    }
    
}
