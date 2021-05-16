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
        public PMSWebApiContext(DbContextOptions<PMSWebApiContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }

        public DbSet<DTOEntities.Task> Tasks { get; set; }

        public DbSet<SubProject> SubProjects { get; set; }

        public DbSet<SubTask> SubTasks { get; set; }

    }
}
