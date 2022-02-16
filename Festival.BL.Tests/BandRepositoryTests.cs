using System;
using System.Linq;
using Xunit;
using Festival.BL.Mappers;
using Festival.BL.Models;
using Festival.BL.Repositories;
using Festival.DAL.Seeds;

namespace Festival.BL.Tests
{
    public class BandRepositoryTests : IDisposable
    {
        private readonly BandRepository _bandRepositorySUT;
        protected readonly DbContextInMemoryFactory _dbContextFactory;

        public BandRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(BandRepositoryTests));
            using var dbx = _dbContextFactory.Create();
            dbx.Database.EnsureCreated();

            _bandRepositorySUT = new BandRepository(_dbContextFactory);
        }

        [Fact]
        public void CreateWithNonExistingItemDoesNotThrow()
        {
            var model = new BandDetailModel()
            {
                Name = "Some band",
                ShortDescription = "Test band"
            };

            var returnedModel = _bandRepositorySUT.InsertUpdate(model);

            Assert.NotNull(returnedModel);
        }

        [Fact]
        public void GetAllSingleSeededBand()
        {
            var band = _bandRepositorySUT
                .GetAll()
                .Single(i => i.Id == Seeds.BandMetallica.Id);

            Assert.Equal(BandMapper.MapBandEntityToListModel(Seeds.BandMetallica), band);
        }

        [Fact]
        public void GetByIdSeededBand()
        {
            var band = _bandRepositorySUT.GetById(Seeds.BandMetallica.Id);

            Assert.Equal(BandMapper.MapBandEntityToDetailModel(Seeds.BandMetallica), band);
        }

        [Fact]
        public void DeleteByIDSeededBand()
        {
            _bandRepositorySUT.Delete(Seeds.BandMetallica.Id);

            using var dbxAssert = _dbContextFactory.Create();

            Assert.False(dbxAssert.Bands.Any(i => i.Id == Seeds.BandMetallica.Id));
        }

        [Fact]
        public void NewBandInsertUpdateBandAdded()
        {
            //Arrange
            var band = new BandDetailModel()
            {
                Name = "Mettalica",
                ShortDescription = "Metal Band"
            };

            //Act
            band = _bandRepositorySUT.InsertUpdate(band);

            //Assert
            using var dbxAssert = _dbContextFactory.Create();
            var bandFromDb = dbxAssert.Bands.Single(i => i.Id == band.Id);
            var bandFromDbModel = BandMapper.MapBandEntityToDetailModel(bandFromDb);
            Assert.Equal(band, bandFromDbModel);
        }

        [Fact]
        public void SeededBandInsertUpdateBandUpdated()
        {
            //Arrange
            var band = new BandDetailModel()
            {
                Id = Seeds.BandMetallica.Id,
                Name = Seeds.BandMetallica.Name,
                PhotoURL = Seeds.BandMetallica.PhotoURL,
                Genre = Seeds.BandMetallica.Genre,
                OriginCountry = Seeds.BandMetallica.OriginCountry,
                ShortDescription = Seeds.BandMetallica.ShortDescription,
                LongDescription = Seeds.BandMetallica.LongDescription,
            };

            band.Name += "updated";
            band.PhotoURL += "updated";
            band.ShortDescription += "updated";
            band.LongDescription += "updated";

            //Act
            _bandRepositorySUT.InsertUpdate(band);

            //Assert
            using var dbxAssert = _dbContextFactory.Create();
            var bandFromDb = dbxAssert.Bands.Single(i => i.Id == band.Id);
            Assert.Equal(band, BandMapper.MapBandEntityToDetailModel(bandFromDb));
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.Create();
            dbx.Database.EnsureDeleted();
        }
    }
}
