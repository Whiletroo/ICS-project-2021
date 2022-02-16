using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Festival.DAL.Factories
{
    public class DesignTimeDbContecFactory : IDesignTimeDbContextFactory<FestivalDbContext>
    {
        public FestivalDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FestivalDbContext>();
            builder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;
                Initial Catalog = Festival;
                MultipleActiveResultSets = True;
                Integrated Security = True; ");

            return new FestivalDbContext(builder.Options);
        }
    }
}
