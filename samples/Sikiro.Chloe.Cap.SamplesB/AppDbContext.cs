using Microsoft.EntityFrameworkCore;

namespace GS.Chloe.Cap.Samples
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public const string ConnectionString = "Server=im.gshichina.com;Port=5002;Database=business_platform;Uid=ge;Pwd=shi2019";

        public DbSet<Person> Persons { get; set; }

    }
}
