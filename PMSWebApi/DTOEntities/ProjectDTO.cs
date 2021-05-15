using PMSWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSWebApi.DTOEntities
{
    public class ProjectDTO
    {
        public  int Id { get; set; }
        public  string Code { get; set; }
        public  string Name { get; set; }
        public  DateTime StartDate { get; set; }
        public  DateTime FinishDate { get; set; }
        public  State State { get; set; }
        public IEnumerable<Model.Task> Tasks { get; set; } 
        public IEnumerable<SubProject> SubProjects { get; set; } 
    }
}
