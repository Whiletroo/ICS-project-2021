using System;
using System.Linq;
using Nemesis.Essentials.Design;
using Festival.DAL.Entity;
using Festival.BL.Models;

namespace Festival.BL.Mappers
{
    public static class StageMapper
    {
        public static StageDetailModel? MapStageEntityToDetailModel(StageEntity? entity)
        {
            return entity == null
                ? null
                : new StageDetailModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    PhotoURL = entity.PhotoURL,
                    ShortDescription = entity.ShortDescription,
                    Performances = new ValueCollection<PerformanceDetailModel>(entity.
                        Performances!.Select(PerformanceMapper.MapPerformanceEntityToDetailModel)
                        .Cast<PerformanceDetailModel>()
                        .ToList())
                };
        }

        public static StageListModel? MapStageEntityToListModel(StageEntity? entity)
        {
            return entity == null
                ? null
                : new StageListModel
                {
                    Id = entity.Id,
                    Name = entity.Name
                };
        }

        public static StageEntity? MapToStageEntity(StageDetailModel? detailModel)
        {
            return detailModel == null
                ? null
                : new StageEntity
                {
                    Id = detailModel.Id,
                    Name = detailModel.Name,
                    PhotoURL = detailModel.PhotoURL,
                    ShortDescription = detailModel.ShortDescription,
                    Performances = new ValueCollection<PerformanceEntity>(detailModel.
                        Performances.Select(PerformanceMapper.MapPerformanceDetailModelToEntity)
                        .Cast<PerformanceEntity>()
                        .ToList())
                };
        }
    }
}
