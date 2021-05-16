using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSWebApi.Model
{
    public class ProjectModel : ProjectBase
    {      
 
        public override int Id { get ; set; }
        [Required]
        public override string Code { get ; set; }
        public override string Name { get; set; }
        public override DateTime StartDate { get; set; }
        public override DateTime FinishDate { get; set; }
        public override State State { get; set ; }
        public IEnumerable<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public IEnumerable<SubProjectModel> SubProjects { get; set; } = new List<SubProjectModel>();
    }
}
