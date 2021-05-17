using Microsoft.EntityFrameworkCore;
using PMSWebApi.DTOEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSWebApi.Data
{
    public class PMSWebApiContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DTOEntities.Task>()                         
                .Property(p => p.SubProjectId)
                .HasColumnName("SubProjectId")
                .HasDefaultValue(null)
                .IsRequired(false);
           
            base.OnModelCreating(modelBuilder);
        }

        public PMSWebApiContext(DbContextOptions<PMSWebApiContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }

        public DbSet<DTOEntities.Task> Tasks { get; set; }

        public DbSet<SubProject> SubProjects { get; set; }

        public DbSet<SubTask> SubTasks { get; set; }

    }
}
