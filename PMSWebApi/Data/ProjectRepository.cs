using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMSWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSWebApi.Data
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PMSWebApiContext _context;
        private readonly ILogger<ProjectRepository> _logger;
        public ProjectRepository(PMSWebApiContext context, ILogger<ProjectRepository> logger)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<Project> GetProjectAsync(string code, bool inculdeTasks = false, bool includeSubProjects = false)
        {
            _logger.LogInformation($"Get project specified by code from DB.");
            return await _context.Projects.Include(a => a.SubProjects).Include(a => a.Tasks).Where(p => p.Code == code).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            _logger.LogInformation($"Get all projects from DB.");
            return await _context.Projects.Include(a => a.SubProjects).Include(a => a.Tasks).ToListAsync();
        }


        public void AddEntity<T>(T entity)
        {
            _logger.LogInformation($"Add new {entity.GetType()} to DB.");
            _context.Add(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Save changes to DB.");
            return (await _context.SaveChangesAsync()) > 0;
        }

        public void DeleteEntity<T>(T entity)
        {
            _logger.LogInformation($"Delete {entity.GetType()} from DB.");
            _context.Remove(entity);
        }
    }
}
