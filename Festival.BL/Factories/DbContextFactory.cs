using System;
using Microsoft.EntityFrameworkCore;
using Festival.DAL;
using Festival.DAL.Factories;

namespace Festival.BL.Factories
{
    public class DbContextFactory : INamedDbContextFactory<FestivalDbContext>
    {
        public FestivalDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<FestivalDbContext>();
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog = TasksDB;MultipleActiveResultSets = True;Integrated Security = True; ");
            return new FestivalDbContext(optionsBuilder.Options);
        }
    }
}
