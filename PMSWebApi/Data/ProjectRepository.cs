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
            IQueryable<Project> query = _context.Projects.Where(x=>x.Code == code);
            if (inculdeTasks)
            {
                query = query.Include(p => p.Tasks);
            }
            if (includeSubProjects)
            {
                query = query.Include(p => p.SubProjects);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync(bool inculdeTasks = false, bool includeSubProjects = false)
        {
            _logger.LogInformation($"Get all projects from DB.");
            IQueryable<Project> query = _context.Projects;
            if (inculdeTasks)
            {
                query = query.Include(p => p.Tasks);
            }
            if (includeSubProjects)
            {
                query = query.Include(p => p.SubProjects);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<SubProject>> GetSubProjectsAsync(string projectCode, bool inculdeTasks = false)
        {
            _logger.LogInformation($"Get all subprojects from DB.");
            IQueryable<SubProject> query = _context.Projects.Where(p => p.Code == projectCode).SelectMany(x => x.SubProjects);
            if (inculdeTasks)
            {
                query = query.Include(p => p.Tasks);
            }

            return await query.ToListAsync();
        }

        public async Task<SubProject> GetSubProjectAsync(string projectCode, string code , bool inculdeTasks = false)
        {
            _logger.LogInformation($"Get subproject specified by id from DB.");
            IQueryable<SubProject> query = _context.Projects.Where(p =>p.Code == projectCode).SelectMany(x => x.SubProjects).Where(y => y.Code == code);
            if (inculdeTasks)
            {
               query = query.Include(p => p.Tasks);
            }
   
            return await query.FirstOrDefaultAsync();
        }

        public Task<IEnumerable<Model.Task>> GetTaskAsync(bool inculdeSubTasks = false)
        {
            throw new NotImplementedException();
        }

        public Task<Model.Task> GetTaskAsync(int id, bool inculdeSubTasks = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SubTask>> GetSubTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Model.Task> GetSubTaskAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void AddEntity<T>(T entity)
        {
            _logger.LogInformation($"Add new {entity.GetType()} to DB.");
            _context.Add(entity);
        }

        public void UpdateEntity<T>(T entity)
        {
            _context.Update(entity);
        }

        public void DeleteEntity<T>(T entity)
        {
            _logger.LogInformation($"Delete {entity.GetType()} from DB.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {

            _logger.LogInformation($"Save changes to DB.");
            return (await _context.SaveChangesAsync()) > 0;
        }


        public void UpdateProjectState(Project project, State state)
        {
            _logger.LogInformation($"Update project state {project.Code}.");
            if (state == State.inProgress)
            {
                project.State = state;
            }
            if (project.Tasks.Where(x => x.State != State.Completed) == null
                || project.SubProjects.Where(x => x.State != State.Completed) == null
                || project.Tasks.SelectMany(s => s.SubTasks).Where(st => st.State != State.Completed) == null)
            {
                project.State = State.Completed;
            }

        }


    }
}
