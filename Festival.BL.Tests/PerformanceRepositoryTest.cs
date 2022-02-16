using System;
using System.Linq;
using Xunit;
using Festival.BL.Mappers;
using Festival.BL.Models;
using Festival.BL.Repositories;
using Festival.DAL.Seeds;
using Festival.BL.Exceptions;
using Festival.DAL.Entity;
using Festival.DAL.Factories;

namespace Festival.BL.Tests
{
    public class PerformaceRepositoryTest : IDisposable
    {
        protected readonly PerformanceRepository _performanceRepositorySUT;
        protected readonly DbContextInMemoryFactory _dbContextFactory;

        public PerformaceRepositoryTest() : base()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(PerformaceRepositoryTest));
            using var dbx = _dbContextFactory.Create();
            dbx.Database.EnsureCreated();
            
            _performanceRepositorySUT = new PerformanceRepository(_dbContextFactory);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.Create();
            dbx.Database.EnsureDeleted();
        }

        [Fact]
        public void CreateWithNonexistingItem()
        {
            var model = new PerformanceDetailModel()
            {
                StartPerformanceTime = new DateTime(2021, 4, 9, 11, 20, 0),
                EndPerformanceTime = new DateTime(2021, 4, 9, 12, 0, 0)
            };

            var returnedModel = _performanceRepositorySUT.InsertUpdate(model);

            Assert.NotNull(returnedModel);
        }

        [Fact]
        public void DateTimeSwap()
        {
            var model = new PerformanceDetailModel()
            {
                StartPerformanceTime = new DateTime(2021, 4, 1, 0, 0, 0),
                EndPerformanceTime = new DateTime(2020, 4, 1, 0, 0, 0)
            };

            Assert.Throws<DateTimeStartEndException>(() => _performanceRepositorySUT.InsertUpdate(model));
        }

        [Fact]
        public void DateTimeNoSwap()
        {
            var model = new PerformanceDetailModel()
            {
                StartPerformanceTime = new DateTime(2020, 4, 1, 0, 0, 0),
                EndPerformanceTime = new DateTime(2021, 4, 1, 0, 0, 0)
            };

            var returnedModel = _performanceRepositorySUT.InsertUpdate(model);

            Assert.NotNull(returnedModel);
        }

        [Fact]
        public void DateTimeCheckConflict()
        {
            using var dbxAssert = _dbContextFactory.Create();
            Assert.Single(dbxAssert.Performances);

            var inDBPerformanceEntity = dbxAssert.Performances.First();

            DateTime endTimeModified = (DateTime)inDBPerformanceEntity.EndPerformanceTime;
            endTimeModified.AddMinutes(30);

            var perfModel = new PerformanceDetailModel()
            {
                StageId = inDBPerformanceEntity.StageId,
                StartPerformanceTime = inDBPerformanceEntity.StartPerformanceTime,
                EndPerformanceTime = endTimeModified
            };

            Assert.Throws<DateTimeCollisionException>(() => _performanceRepositorySUT.InsertUpdate(perfModel));
            Assert.Single(dbxAssert.Performances);
        }

        [Fact]
        public void DateTimeCheckNoConflict()
        {
            using var dbxAssert = _dbContextFactory.Create();
            Assert.Single(dbxAssert.Performances);

            var inDBStageId = dbxAssert.Performances.First().StageId;

            var perfModel1 = new PerformanceDetailModel()
            {
                StageId = inDBStageId,
                StartPerformanceTime = new DateTime(2021, 4, 9, 0, 0, 0),
                EndPerformanceTime = new DateTime(2021, 4, 10, 0, 0, 0)
            };
            

            var returnedModel = _performanceRepositorySUT.InsertUpdate(perfModel1);

            Assert.NotNull(returnedModel);
            Assert.Equal(2, dbxAssert.Performances.Count());
        }

        [Fact]
        public void GetAllSingleSeeded()
        {
            var performance = _performanceRepositorySUT
                .GetAll()
                .Single(i => i.Id == Seeds.PerformanceMetallica.Id);

            Assert.Equal(PerformanceMapper.MapPerformanceEntityToListModel(Seeds.PerformanceMetallica), performance);
        }

        [Fact]
        public void GetByIdSeeded()
        {
            var performance = _performanceRepositorySUT.GetById(Seeds.PerformanceMetallica.Id);

            Assert.Equal(PerformanceMapper.MapPerformanceEntityToDetailModel(Seeds.PerformanceMetallica), performance);
        }

        [Fact]
        public void NewPerformanceInsertUpdate()
        {
            //Arrange
            var performance = new PerformanceDetailModel()
            {
                //mozna pridat idcka?
                StartPerformanceTime = new DateTime(2020, 01, 01, 20, 00, 00),
                EndPerformanceTime = new DateTime(2020, 01, 01, 21, 30, 00),
            };

            //Act
            performance = _performanceRepositorySUT.InsertUpdate(performance);

            //Assert
            using var dbxAssert = _dbContextFactory.Create();
            var perfFromDb = dbxAssert.Performances.Single(i => i.Id == performance.Id);
            Assert.Equal(performance, PerformanceMapper.MapPerformanceEntityToDetailModel(perfFromDb));
        }

        [Fact]
        public void SeededPerformanceInsertUpdate()
        {
            //Arrange
            var performance = new PerformanceDetailModel()
            {
                Id = Seeds.PerformanceMetallica.Id,
                StartPerformanceTime = Seeds.PerformanceMetallica.StartPerformanceTime,
                EndPerformanceTime = Seeds.PerformanceMetallica.EndPerformanceTime,
                BandId = Seeds.PerformanceMetallica.BandId,
                StageId = Seeds.PerformanceMetallica.StageId
            };

            performance.StartPerformanceTime = new DateTime(2021, 05, 01, 22, 00, 00);
            performance.EndPerformanceTime = new DateTime(2021, 05, 01, 22, 30, 00);

            //Act
            _performanceRepositorySUT.InsertUpdate(performance);

            //Assert
            using var dbxAssert = _dbContextFactory.Create();
            var perfFromDb = dbxAssert.Performances.Single(i => i.Id == performance.Id);
            Assert.Equal(performance, PerformanceMapper.MapPerformanceEntityToDetailModel(perfFromDb));
        }

        [Fact]
        public void DeleteByIDSeeded()
        {
            _performanceRepositorySUT.Delete(Seeds.PerformanceMetallica.Id);

            using var dbContext = _dbContextFactory.Create();

            Assert.False(dbContext.Bands.Any(i => i.Id == Seeds.PerformanceMetallica.Id));
        }
    }
}