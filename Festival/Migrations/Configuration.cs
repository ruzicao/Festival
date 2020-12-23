namespace Festival.Migrations
{
    using Festival.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Festival.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Festival.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Places.AddOrUpdate(x => x.Id,
                new Place() { Id = 1, Location = "Beograd", ZipCode = 11000 },
                new Place() { Id = 2, Location = "Novi Sad", ZipCode = 21000 },
                new Place() { Id = 3, Location = "Subotica", ZipCode = 12000 }
            );
            context.SaveChanges();

            context.Events.AddOrUpdate(x => x.Id,
                new Event() { Id = 1, Name = "Hello Summer", Year = 2017, Price = 2000.00M, PlaceId = 1 },
                new Event() { Id = 2, Name = "Autumn18", Year = 2017, Price = 2500.00M, PlaceId = 2 },
                new Event() { Id = 3, Name = "WinterFest", Year = 2016, Price = 3000.00M, PlaceId = 3 }
            );
            context.SaveChanges();

        }
    }
}
