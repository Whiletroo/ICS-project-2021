using System;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Festival.DAL.Entity;

namespace Festival.DAL.Tests
{
    public class StageEntityTests : FestivalDbContextTests
    {
        [Fact]
        public void AddStageTest()
        {
            //Arrange
            var stageEntity = new StageEntity
            {
                Name = "Stage A",
                PhotoURL = "../StageA.png",
                ShortDescription = "Main stage"
            };
            //Act
            _dbContextSUT.Stages.Add(stageEntity);
            _dbContextSUT.SaveChanges();

            //Assert
            using var dbx = _dbContextFactory.Create();
            var retrievedStage = dbx.Stages.Single(entity => entity.Id == stageEntity.Id);
            Assert.Equal(stageEntity,retrievedStage);
        }

        [Fact]
        public void AddStageTest_WithPerformances()
        {
            //Arrange
            var performanceEntity = new PerformanceEntity
            {
                StartPerformanceTime = new DateTime(2021, 7, 17, 19, 30, 0),
                EndPerformanceTime = new DateTime(2021, 7, 17, 21, 0, 0)
            };


            var stageEntity = new StageEntity
            {
                Name = "Stage A",
                PhotoURL = "../StageA.png",
                ShortDescription = "Main stage"
            };

            stageEntity.Performances.Add(performanceEntity);

            //Act
            _dbContextSUT.Stages.Add(stageEntity);
            _dbContextSUT.SaveChanges();

            //Assert
            using var dbx = _dbContextFactory.Create();
            var retrievedStage = dbx.Stages
                .Include(entity => entity.Performances)
                .First(entity => entity.Id == stageEntity.Id);

            Assert.Equal(stageEntity, retrievedStage);
        }

        [Fact]
        public void DeleteStageTest()
        {
            //Arrange
            using var dbx = _dbContextFactory.Create();
            var stageEntity = new StageEntity
            {
                Id = new Guid(),
                Name = "Stage A"
            };

            //Act
            dbx.Attach(stageEntity);
            dbx.Remove(stageEntity);
            dbx.SaveChanges();

            //Assert
            Assert.Null(dbx.Stages.Find(stageEntity.Id));
        }

        [Fact]
        public void UpdateStageTest()
        {
            //Arrange
            using var dbx = _dbContextFactory.Create();
            var stageEntity = new StageEntity
            {
                Id = new Guid(),
                Name = "Stage A"
            };

            //Act
            dbx.Attach(stageEntity);
            dbx.Add(stageEntity);
            dbx.SaveChanges();
            dbx.Update(dbx.Stages.Find(stageEntity.Id));
            dbx.Stages.Find(stageEntity.Id).Name = "Stage B";
            dbx.SaveChanges();

            var updateStageEntity = "Stage B";

            //Assert
            Assert.Equal(updateStageEntity, dbx.Stages.Find(stageEntity.Id).Name);
        }
    }
}
