using FlyPlan.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyPlan.Api.UnitTests
{
    public static class FlyplanDbContextMocker
    {
        public static FlyplanContext GetFlyplanContext(string dbName)
        {
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<FlyplanContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // Create instance of DbContext
            var dbContext = new FlyplanContext(options);

            // Add entities in memory
            dbContext.Seed();

            return dbContext;
        }
    }
}
