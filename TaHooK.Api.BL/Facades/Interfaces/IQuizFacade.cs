﻿using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Models.Quiz;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface IQuizFacade : ICrudFacade<QuizEntity, QuizListModel, QuizDetailModel, QuizCreateUpdateModel>
{
}