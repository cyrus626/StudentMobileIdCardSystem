using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MobileCard.API.Models.Entities;

namespace MobileCard.API.Services
{
    public class ApplicationContext : IdentityDbContext, IDesignTimeDbContextFactory<ApplicationContext>
    {
        #region Properties
        public DbSet<EnrollmentApplication> EnrollmentApplications { get; set; }
        public DbSet<Resource> Resources { get; set; }
        #endregion

        #region Constructors
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = new SqliteConnectionStringBuilder()
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                DataSource = Core.DATABASE_PATH,
            }.ConnectionString;

            optionsBuilder.UseSqlite(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        public ApplicationContext CreateDbContext(string[] args)
        {
            ApplicationContext context = new ApplicationContext();
            var builder = new DbContextOptionsBuilder();
            context.OnConfiguring(builder);

            return context;
        }
    }
}
