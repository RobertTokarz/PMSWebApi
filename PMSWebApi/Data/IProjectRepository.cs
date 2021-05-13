using PMSWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSWebApi.Data
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<Project> GetProjectAsync(string code, bool inculdeTasks = false, bool includeSubProjects = false);


        void AddEntity<T>(T entity);
        void DeleteEntity<T>(T entity);
        Task<bool> SaveChangesAsync();
    }
}
