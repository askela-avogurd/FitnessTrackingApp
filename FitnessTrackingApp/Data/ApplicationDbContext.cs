using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using FitnessTrackingApp.Models;

namespace FitnessTrackingApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { } 

        public DbSet<TrainingProgram> TrainingProgram { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<Activity> Activity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Таблицы БД будут называться в соответствии с названиями моделей в коде (в данном случае в единственном числе).
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
