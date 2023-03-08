namespace LMS_library.Data
{
    public class DataDBContex : DbContext
    {

        public DataDBContex(DbContextOptions<DataDBContex> options) :base(options) 
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Connect"));
        }

        public DbSet<User> Users { get; set; } 
    }
}
