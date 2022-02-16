using System;
using Microsoft.EntityFrameworkCore;
using Festival.DAL;
using Festival.DAL.Factories;

namespace Festival.BL.Tests
{
    public class DbContextInMemoryFactory : INamedDbContextFactory<FestivalDbContext>
    {
        private readonly string _databaseName;

        public DbContextInMemoryFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public FestivalDbContext Create()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<FestivalDbContext>();
            contextOptionsBuilder.UseInMemoryDatabase(_databaseName);
            return new FestivalDbContext(contextOptionsBuilder.Options);
        }
    }
}
