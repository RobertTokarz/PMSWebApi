using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMSWebApi.DTOEntities;
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
            IQueryable<DTOEntities.Project> query = _context.Projects.Where(x => x.Code == code);
            if (inculdeTasks)
            {
                query = query.Include(p => p.Tasks.Where(t => t.ProjectType == ProjectType.Main));
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
            IQueryable<DTOEntities.Project> query = _context.Projects;
            if (inculdeTasks)
            {
                query = query.Include(p => p.Tasks.Where(t=>t.ProjectType == ProjectType.Main));
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
                query = query.Include(p => p.Tasks.Where(t => t.ProjectType == ProjectType.Sub));
            }

            return await query.ToListAsync();
        }

        public async Task<SubProject> GetSubProjectAsync(string projectCode, int id , bool inculdeTasks = false)
        {
            _logger.LogInformation($"Get subproject specified by id from DB.");
            IQueryable<SubProject> query = _context.Projects.Where(p =>p.Code == projectCode).SelectMany(x => x.SubProjects).Where(y => y.Id == id);
            if (inculdeTasks)
            {
               query = query.Include(p => p.Tasks.Where(t => t.ProjectType == ProjectType.Sub));
            }
   
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DTOEntities.Task>> GetTasksAsync(string projectCode, bool inculdeSubTasks = false)
        {
            _logger.LogInformation($"Get all tasks from DB.");
            IQueryable<DTOEntities.Task> query = _context.Projects.Where(p => p.Code == projectCode).SelectMany(x => x.Tasks);
            if (inculdeSubTasks)
            {
                query = query.Include(p => p.SubTasks);
            }

            return await query.ToListAsync();
        }

        public async Task<DTOEntities.Task> GetTaskAsync(int id, bool inculdeSubTasks = false)
        {
            _logger.LogInformation($"Get task specified by id from DB.");
            IQueryable<DTOEntities.Task> query = _context.Tasks.Where(p => p.Id == id);
            if (inculdeSubTasks)
            {
                query = query.Include(p => p.SubTasks);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SubTask>> GetSubTasksAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<SubTask> GetSubTaskAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void AddEntity<T>(T entity)
        {
            _logger.LogInformation($"Add new {entity.GetType()} to DB.");
            _context.Add(entity);
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
          
            else if (!project.Tasks.Any(x => x.State != State.Completed)
                && !project.SubProjects.Any(x => x.State != State.Completed)
                && !project.Tasks.SelectMany(s => s.SubTasks).Any(st => st.State != State.Completed))
            {
                project.State = State.Completed;
            }

        }


    }
}
