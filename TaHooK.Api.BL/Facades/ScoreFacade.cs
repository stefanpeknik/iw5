﻿using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Score;

namespace TaHooK.Api.BL.Facades;

public class ScoreFacade: FacadeBase<ScoreEntity, ScoreListModel, ScoreDetailModel>, IScoreFacade
{
    public ScoreFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }
}