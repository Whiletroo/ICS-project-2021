using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Festival.BL.Mappers;
using Festival.BL.Models;
using Festival.DAL;
using Festival.DAL.Factories;
using Festival.DAL.Entity;

namespace Festival.BL.Repositories
{
    public class BandRepository : RepositoryBase, IRepository<BandListModel, BandDetailModel>
    {
        public BandRepository(INamedDbContextFactory<FestivalDbContext> dbContextFactory) : base(dbContextFactory) { }

        public IEnumerable<BandListModel> GetAll()
        {
            using var dbContext = _dbContextFactory.Create();
            return dbContext.Bands.Select(e => BandMapper.MapBandEntityToListModel(e)!).ToArray();
        }

        public BandDetailModel GetById(Guid id)
        {
            using var dbContext = _dbContextFactory.Create();

            IQueryable<BandEntity> query = dbContext.Set<BandEntity>();
            query = query.Include(entity => entity.Performances).ThenInclude(entity => entity.Stage);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return BandMapper.MapBandEntityToDetailModel(entity)!;
        }

        public BandDetailModel InsertUpdate(BandDetailModel model)
        {
            using var dbContext = _dbContextFactory.Create();
            var entity = BandMapper.MapBandDetailModelToEntity(model);
            dbContext.Bands.Update(entity!);
            dbContext.SaveChanges();
            return BandMapper.MapBandEntityToDetailModel(entity)!;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.Create();
            var entity = new BandEntity
            {
                Id = id
            };
            dbContext.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
