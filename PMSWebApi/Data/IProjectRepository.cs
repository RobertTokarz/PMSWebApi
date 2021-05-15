using PMSWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSWebApi.Data
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsAsync(bool inculdeTasks = false, bool includeSubProjects = false);
        Task<Project> GetProjectAsync(string code, bool inculdeTasks = false, bool includeSubProjects = false);

        Task<IEnumerable<SubProject>> GetSubProjectsAsync(string projectCode,  bool inculdeTasks = false);
        Task<SubProject> GetSubProjectAsync(string projectCode, string code, bool inculdeTasks = false);

        Task<IEnumerable<Model.Task>> GetTaskAsync(bool inculdeSubTasks = false);
        Task<Model.Task> GetTaskAsync(int id, bool inculdeSubTasks = false);

        Task<IEnumerable<SubTask>> GetSubTasksAsync();
        Task<Model.Task> GetSubTaskAsync(int id);

        void AddEntity<T>(T entity);
        void DeleteEntity<T>(T entity);
        void UpdateEntity<T>(T entity);
        Task<bool> SaveChangesAsync();

        void UpdateProjectState(Project project, State state);
    }
}
