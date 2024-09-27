using Microsoft.EntityFrameworkCore;
using TaskFlowModels.Models;

namespace TaskFlowSqlite.DataAccess
{
    public class TaskFlowDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<ListItem> Lists { get; set; }
        public string DbPath { get; }

        public TaskFlowDbContext()
        {
        }

        public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "taskflow.db");

            Database.EnsureCreated();
            //Database.EnsureDeleted();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed base lists
            modelBuilder.Entity<ListItem>().HasData(SeedData.GetSeedListData());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
}
