using System;
using Microsoft.EntityFrameworkCore;
using Nemesis.Essentials.Design;
using Festival.DAL.Entity;

namespace Festival.DAL.Seeds
{
    public static class Seeds
    {
        public static readonly BandEntity BandMetallica = new()
        {
            Id = Guid.Parse("1cb6fb2f-6c01-4bb4-b0b6-cb0d39c75daa"),
            Name = "Metallica",
            PhotoURL = "MetallicaURL",
            Genre = Genres.Metal,
            OriginCountry = Countries.US,
            ShortDescription = "American heavy metal band",
            LongDescription = "The band was formed in 1981 in Los Angeles by vocalist/guitarist James Hetfield and drummer Lars Ulrich, and has been based in San Francisco for most of its career. " +
                              "The band's fast tempos, instrumentals and aggressive musicianship made them one of the founding big four bands of thrash metal, alongside Megadeth, Anthrax and Slayer. " +
                              "Metallica's current lineup comprises founding members and primary songwriters Hetfield and Ulrich, longtime lead guitarist Kirk Hammett, and bassist Robert Trujillo. " +
                              "Guitarist Dave Mustaine (who went on to form Megadeth after being fired from the band) and bassists Ron McGovney, Cliff Burton (who died in a bus accident in Sweden in 1986) and Jason Newsted are former members of the band.",
        };

        public static readonly StageEntity StageA = new()
        {
            Id = Guid.Parse("bdbfaf5b-fe9f-4a01-be3e-5d41b2d5cc5b"),
            Name = "Stage A",
            PhotoURL = "StageAURL",
            ShortDescription = "Main Stage",
    };

        public static readonly PerformanceEntity PerformanceMetallica = new()
        {
            Id = Guid.Parse("040fc4f3-6f46-40b4-83d8-90833cdf44a8"),
            Band = BandMetallica,
            BandId = BandMetallica.Id,
            Stage = StageA,
            StageId = StageA.Id,
            StartPerformanceTime = new DateTime(2021, 05, 06, 20, 00, 00),
            EndPerformanceTime = new DateTime(2021, 05, 06, 21, 30, 00),
        };

        static Seeds()
        {
            BandMetallica.Performances = new ValueCollection<PerformanceEntity>
            {
                PerformanceMetallica
            };
            StageA.Performances = new ValueCollection<PerformanceEntity>
            {
                PerformanceMetallica
            };
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BandEntity>().HasData(new
            {
                Id = BandMetallica.Id,
                Name = BandMetallica.Name,
                PhotoURL = BandMetallica.PhotoURL,
                Genre = BandMetallica.Genre,
                OriginCountry = BandMetallica.OriginCountry,
                ShortDescription = BandMetallica.ShortDescription,
                LongDescription = BandMetallica.LongDescription
            });
            modelBuilder.Entity<StageEntity>().HasData(new
            {
                Id = StageA.Id,
                Name = StageA.Name,
                PhotoURL = StageA.PhotoURL,
                ShortDescription = StageA.ShortDescription
            });
            modelBuilder.Entity<PerformanceEntity>().HasData(new
            {
                Id = PerformanceMetallica.Id,
                StartPerformanceTime = PerformanceMetallica.StartPerformanceTime,
                EndPerformanceTime = PerformanceMetallica.EndPerformanceTime,
                BandId = BandMetallica.Id,
                StageId = StageA.Id
            });
        }
    }
}
