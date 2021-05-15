using PMSWebApi.Data;
using PMSWebApi.Model;
using System;
using System.Net.Http;
using Xunit;

namespace PMSWebApi.Test
{
    public class ProjectTest
    {
        private IProjectRepository _projectRepository;
        public ProjectTest(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [Fact]
        public void Test1()
        {
           
            Project project = new Project
            {                
                Code = "123",
                Name = "test",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddDays(10),
                State = 0
            };
            Assert.NotNull(project);

        }

        public void Test2()
        {

            Project project = new Project
            {
                Code = "123",
                Name = "test",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddDays(10),
                State = 0
            };
            Assert.NotNull(project);

        }

    }
}
