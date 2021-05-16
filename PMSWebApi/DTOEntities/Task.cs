using PMSWebApi.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMSWebApi.DTOEntities
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int Id { get; set; }
        public  string Name { get; set; }
        public  string Description { get; set; }
        public  DateTime StartDate { get; set; }
        public  DateTime FinishDate { get; set; }
        public  State State { get; set; }
        public IEnumerable<SubTask> SubTasks { get; set; } 

        public int ProjectId { get; set; }
        public ProjectType ProjectType { get; set; }

        public int? SubProjectId { get; set; }

    }
}
