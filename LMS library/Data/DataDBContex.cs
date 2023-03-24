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

        public DbSet<User> Users {get; set;}
        public DbSet<Role> Roles { get; set; }
        public DbSet<PrivateFiles> PrivateFiles { get; set; }
        public DbSet<SystemDetail> System { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseMaterial> Materials { get; set; }
        public DbSet<MaterialTopic> Topics { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
    }
}
