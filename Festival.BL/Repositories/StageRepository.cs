using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Festival.BL.Mappers;
using Festival.BL.Models;
using Festival.DAL;
using Festival.DAL.Entity;
using Festival.DAL.Factories;

namespace Festival.BL.Repositories
{
    public class StageRepository : RepositoryBase, IRepository<StageListModel, StageDetailModel>
    {
        public StageRepository(INamedDbContextFactory<FestivalDbContext> dbContextFactory) : base(dbContextFactory) { }
        public IEnumerable<StageListModel> GetAll()
        {
            using var dbContext = _dbContextFactory.Create();
            return dbContext.Stages.Select(e => StageMapper.MapStageEntityToListModel(e)!).ToArray();
        }
        public StageDetailModel GetById(Guid id)
        {
            using var dbContext = _dbContextFactory.Create();
            IQueryable<StageEntity> query = dbContext.Set<StageEntity>();
            query = query.Include(entity => entity.Performances).ThenInclude(entity => entity.Band);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return StageMapper.MapStageEntityToDetailModel(entity)!;
        }
        public StageDetailModel InsertUpdate(StageDetailModel model)
        {
            using var dbContext = _dbContextFactory.Create();
            var entity = StageMapper.MapToStageEntity(model);
            dbContext.Stages.Update(entity!);
            dbContext.SaveChanges();
            return StageMapper.MapStageEntityToDetailModel(entity)!;
        }
        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.Create();
            var entity = new StageEntity()
            {
                Id = id
            };
            dbContext.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
