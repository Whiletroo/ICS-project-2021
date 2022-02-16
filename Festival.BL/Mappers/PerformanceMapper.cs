using System;
using Festival.BL.Models;
using Festival.DAL.Entity;

namespace Festival.BL.Mappers
{
    public static class PerformanceMapper
    {
        public static PerformanceListModel? MapPerformanceEntityToListModel(PerformanceEntity? entity)
        {
            return entity == null ? null : new PerformanceListModel
            {
                Id = entity.Id,
                BandName = entity.Band?.Name,
                StageName = entity.Stage?.Name,
                Genre = entity.Band == null ? Genres.None : entity.Band.Genre,
                StartPerformanceTime = entity.StartPerformanceTime,
                EndPerformanceTime = entity.EndPerformanceTime
            };
        }

        public static PerformanceDetailModel? MapPerformanceEntityToDetailModel(PerformanceEntity? entity)
        {
            return entity == null ? null : new PerformanceDetailModel
            {
                Id = entity.Id,
                BandId = entity.BandId,
                BandName = entity.Band?.Name,
                StageId = entity.StageId,
                StageName = entity.Stage?.Name,
                StartPerformanceTime = entity.StartPerformanceTime,
                EndPerformanceTime = entity.EndPerformanceTime
            };
        }

        public static PerformanceEntity? MapPerformanceDetailModelToEntity(PerformanceDetailModel? model)
        {
            return model == null ? null : new PerformanceEntity
            {
                Id = model.Id,
                BandId = model.BandId,
                //Band = BandMapper.MapPerformanceDetailModelToEntity(model),
                StageId = model.StageId,
                //Stage = StageMapper.MapPerformanceDetailModelToEntity(model),
                StartPerformanceTime = model.StartPerformanceTime,
                EndPerformanceTime = model.EndPerformanceTime
            };
        }
    }
}
