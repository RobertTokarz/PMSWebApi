using PMSWebApi.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMSWebApi.DTOEntities
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int Id { get; set; }
        public  string Code { get; set; }
        public  string Name { get; set; }
        public  DateTime StartDate { get; set; }
        public  DateTime FinishDate { get; set; }
        public  State State { get; set; }
        public IEnumerable<Task> Tasks { get; set; } 
        public IEnumerable<SubProject> SubProjects { get; set; } 
    }
}
