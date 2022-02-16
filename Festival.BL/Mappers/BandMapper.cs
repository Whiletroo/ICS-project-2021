using System;
using System.Linq;
using Nemesis.Essentials.Design;
using Festival.BL.Models;
using Festival.DAL.Entity;

namespace Festival.BL.Mappers
{
    public class BandMapper
    {
        public static BandListModel? MapBandEntityToListModel(BandEntity? entity)
        {
            return entity == null ? null : new BandListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Genre = entity.Genre,
                ShortDescription = entity.ShortDescription
            };
        }

        public static BandDetailModel? MapBandEntityToDetailModel(BandEntity? entity)
        {
            return entity == null ? null : new BandDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Genre = entity.Genre,
                PhotoURL = entity.PhotoURL,
                OriginCountry = entity.OriginCountry,
                ShortDescription = entity.ShortDescription,
                LongDescription= entity.LongDescription,
                Performances = new ValueCollection<PerformanceDetailModel>(entity.
                    Performances!.Select(PerformanceMapper.MapPerformanceEntityToDetailModel)
                    .Cast<PerformanceDetailModel>()
                    .ToList())
            };
        }

        public static BandEntity? MapBandDetailModelToEntity(BandDetailModel? model)
        {
            return model == null ? null : new BandEntity
            {
                Id = model.Id,
                Name = model.Name,
                Genre = model.Genre,
                PhotoURL = model.PhotoURL,
                OriginCountry = model.OriginCountry,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
                Performances = new ValueCollection<PerformanceEntity>(model.
                    Performances.Select(PerformanceMapper.MapPerformanceDetailModelToEntity)
                    .Cast<PerformanceEntity>()
                    .ToList())
            };
        }
    }
}

