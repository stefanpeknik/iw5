using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaHooK.Common.Models;
using Microsoft.Extensions.Options;
using TaHooK.Common.Models.Quiz;

namespace TaHooK.Web.BL.Facades;

// public class QuizFacade : FacadeBase<QuizDetailModel, QuizListModel>
// {
//     private readonly IQuizApiClient apiClient;
//     
//     
//     public QuizFacade(
//         IQuizApiClient apiClient,
//         QuizRepository QuizRepository,
//         IMapper mapper,
//         IOptions<LocalDbOptions> localDbOptions)
//         : base(QuizRepository, mapper, localDbOptions)
//     {
//         this.apiClient = apiClient;
//     }
// }