using System;

namespace Festival.DAL.Tests
{
    public class FestivalDbContextTests : IDisposable
    {
        protected readonly DbContextInMemoryFactory _dbContextFactory;
        protected readonly FestivalDbContext _dbContextSUT;

        public FestivalDbContextTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(FestivalDbContextTests));
            _dbContextSUT = _dbContextFactory.Create();
        }

        public void Dispose()
        {
            _dbContextSUT.Dispose();
        }
    }
}
