using System;
using Festival.DAL;
using Festival.DAL.Factories;

namespace Festival.BL.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly INamedDbContextFactory<FestivalDbContext> _dbContextFactory;

        public RepositoryBase(INamedDbContextFactory<FestivalDbContext> dbContextFactory)
        {
            this._dbContextFactory = dbContextFactory;
        }
    }
}
