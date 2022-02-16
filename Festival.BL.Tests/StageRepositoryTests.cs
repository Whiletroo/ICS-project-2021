using System;
using System.Linq;
using Xunit;
using Festival.BL.Mappers;
using Festival.BL.Models;
using Festival.BL.Repositories;
using Festival.DAL.Seeds;

namespace Festival.BL.Tests
{
    public class StageRepositoryTests : IDisposable
    {
        private readonly StageRepository _stageRepositorySUT;
        protected readonly DbContextInMemoryFactory _dbContextFactory;

        public StageRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(StageRepositoryTests));
            using var dbx = _dbContextFactory.Create();
            dbx.Database.EnsureCreated();

            _stageRepositorySUT = new StageRepository(_dbContextFactory);
        }

        [Fact]
        public void Create()
        {
            var model = new StageDetailModel
            {
                Name = "Stage",
                PhotoURL = "Stage.png",
                ShortDescription = "Test stage"
            };
            var returnedModel = _stageRepositorySUT.InsertUpdate(model);
            Assert.NotNull(returnedModel);
        }

        [Fact]
        public void GetAllSingleSeededStage()
        {
            var stage = _stageRepositorySUT
                .GetAll()
                .Single(i => i.Id == Seeds.StageA.Id);

            Assert.Equal(StageMapper.MapStageEntityToListModel(Seeds.StageA), stage);
        }

        [Fact]
        public void GetByIdSeededStage()
        {
            var stage = _stageRepositorySUT.GetById(Seeds.StageA.Id);
            var stageA = StageMapper.MapStageEntityToDetailModel(Seeds.StageA);

            Assert.Equal(stageA, stage);
        }

        [Fact]
        public void SeededStageDeleteById()
        {
            _stageRepositorySUT.Delete(Seeds.StageA.Id);

            using var dbxAssert = _dbContextFactory.Create();
            Assert.False(dbxAssert.Stages.Any(i => i.Id == Seeds.StageA.Id));
        }

        [Fact]
        public void InsertUpdateStageAdded()
        {
            var stage = new StageDetailModel()
            {
                Name = "Stage B",
                PhotoURL = "StageB.png",
                ShortDescription = "Second stage"
            };
            
            stage = _stageRepositorySUT.InsertUpdate(stage);
            
            using var dbxAssert = _dbContextFactory.Create();
            var stageFromDb = dbxAssert.Stages.Single(i => i.Id == stage.Id);
            Assert.Equal(stage, StageMapper.MapStageEntityToDetailModel(stageFromDb));
        }

        [Fact]
        public void InsertUpdateStageUpdated()
        {
            var stage = new StageDetailModel()
            {
                Id = Seeds.StageA.Id,
                Name = Seeds.StageA.Name,
                PhotoURL = Seeds.StageA.PhotoURL,
                ShortDescription = Seeds.StageA.ShortDescription
            };
            stage.Name += "updated";
            stage.PhotoURL = "StageUpdated.pgn";
            stage.ShortDescription += "updated";
            
            _stageRepositorySUT.InsertUpdate(stage);
            
            using var dbxAssert = _dbContextFactory.Create();
            var stageFromDb = dbxAssert.Stages.Single(i => i.Id == stage.Id);
            var stageX = StageMapper.MapStageEntityToDetailModel(stageFromDb);
            Assert.Equal(stage, StageMapper.MapStageEntityToDetailModel(stageFromDb));
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.Create();
            dbx.Database.EnsureDeleted();
        }
    }
}
