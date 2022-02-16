using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Festival.BL.Mappers;
using Festival.BL.Models;
using Festival.DAL;
using Festival.DAL.Entity;
using Festival.DAL.Factories;
using Festival.BL.Exceptions;

namespace Festival.BL.Repositories
{
    public class PerformanceRepository : RepositoryBase, IRepository<PerformanceListModel, PerformanceDetailModel>
    {
        public PerformanceRepository(INamedDbContextFactory<FestivalDbContext> dbContextFactory) : base(dbContextFactory) { }

        public IEnumerable<PerformanceListModel> GetAll()
        {
            using var dbContext = _dbContextFactory.Create();
            IQueryable<PerformanceEntity> query = dbContext.Set<PerformanceEntity>();
            query = query.Include(entity => entity.Band).Include(entity => entity.Stage);
            return query.AsEnumerable().Select(e => PerformanceMapper.MapPerformanceEntityToListModel(e)!).ToArray();
        }

        public PerformanceDetailModel GetById(Guid id)
        {
            using var dbContext = _dbContextFactory.Create();
            IQueryable<PerformanceEntity> query = dbContext.Set<PerformanceEntity>();
            query = query.Include(entity => entity.Band).Include(entity => entity.Stage);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return PerformanceMapper.MapPerformanceEntityToDetailModel(entity)!;
        }

        private bool CheckTimestampCollision(PerformanceDetailModel model)
        {
            var dbContext = _dbContextFactory.Create();
            var performanceConflict = dbContext.Performances.Where(p =>
                p.StageId == model.StageId &&
                (p.StartPerformanceTime >= model.StartPerformanceTime 
                && p.StartPerformanceTime < model.EndPerformanceTime
                || p.EndPerformanceTime <= model.EndPerformanceTime
                && p.EndPerformanceTime > model.StartPerformanceTime)).Any();

            return performanceConflict;
        }

        private bool CheckTimestampStartEnd(PerformanceDetailModel model)
        {
            return model.StartPerformanceTime > model.EndPerformanceTime;
        }

        public PerformanceDetailModel InsertUpdate(PerformanceDetailModel model)
        {
            if (CheckTimestampStartEnd(model))
            {
                throw new DateTimeStartEndException();
            }

            if (CheckTimestampCollision(model))
            {
                throw new DateTimeCollisionException();
            }

            using var dbContext = _dbContextFactory.Create();
            var entity = PerformanceMapper.MapPerformanceDetailModelToEntity(model);
            dbContext.Performances.Update(entity!);
            dbContext.SaveChanges();
            return PerformanceMapper.MapPerformanceEntityToDetailModel(entity)!;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.Create();
            var entity = new PerformanceEntity
            {
                Id = id
            };
            dbContext.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
