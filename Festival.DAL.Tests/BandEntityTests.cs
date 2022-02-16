using System;
using System.Linq;
using Festival.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Festival.DAL.Tests
{
    public class BandEntityTests : FestivalDbContextTests
    {
        [Fact]
        public void AddNewBandTest()
        {
            var myNewBand = new BandEntity()
            {
                //Arrange
                Name = "ACDC",
                PhotoURL = "some URL",
                Genre = Genres.Rock,
                OriginCountry = Countries.AU,
                ShortDescription = "Legendary rock band",
                LongDescription = "AC/DC are an Australian rock band, that is active from 1973."
            };

            //Act
            _dbContextSUT.Bands.Add(myNewBand);
            _dbContextSUT.SaveChanges();

            //Assert
            using var dbx = _dbContextFactory.Create();
            var retrievedBand = dbx.Bands.Single(entity => entity.Id == myNewBand.Id);
            Assert.Equal(myNewBand, retrievedBand);
        }

        [Fact]
        public void AddNewBand_WithPerformanceTest()
        {
            var myNewBand = new BandEntity()
            {
                //Arrange
                Name = "Little Big",
                PhotoURL = "some URL",
                Genre = Genres.Other,
                OriginCountry = Countries.RU,
                ShortDescription = "Russian rave band",
                LongDescription = "Little Big is a Russian world known rave band."
            };

            var myPerformance = new PerformanceEntity()
            {
                StartPerformanceTime = new DateTime(2021, 08, 08, 19, 00, 00),
                EndPerformanceTime = new DateTime(2021, 08, 08, 20, 30, 00)
            };

            myNewBand.Performances.Add(myPerformance);

            //Act
            _dbContextSUT.Bands.Add(myNewBand);
            _dbContextSUT.SaveChanges();

            //Assert
            using var dbx = _dbContextFactory.Create();
            var retrievedBand = dbx.Bands
                .Include(Entity => Entity.Performances)
                .Single(Entity => Entity.Id == myNewBand.Id);

            Assert.Equal(myNewBand, retrievedBand);
        }

        [Fact]
        public void DeleteBandTest()
        {
            //Arrenge
            using var dbx = _dbContextFactory.Create();
            var band = new BandEntity()
            {
                Id = new Guid(),
                Name = "ACDC"
            };
            
            //Act
            dbx.Attach(band);
            dbx.Remove(band);
            dbx.SaveChanges();

            //Assert
            Assert.Null(dbx.Bands.Find(band.Id));
        }

        [Fact]
        public void UpdateBandTest()
        {
            //Arrange
            using var dbx = _dbContextFactory.Create();
            var band = new BandEntity()
            {
                Id = new Guid(),
                Name = "Deep Purple"
            };
            
            //Act
            dbx.Attach(band);
            dbx.Add(band);
            dbx.SaveChanges();
            dbx.Update(dbx.Bands.Find(band.Id));
            dbx.Bands.Find(band.Id).Name = "High Green";
            dbx.SaveChanges();
                
            var bandUpdate = "High Green";
                
            //Assert
            Assert.Equal(bandUpdate, dbx.Bands.Find(band.Id).Name);
        }
    }
}
