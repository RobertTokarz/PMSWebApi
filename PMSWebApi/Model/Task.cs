using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSWebApi.Model
{
    public class Task: TaskBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override  DateTime StartDate { get; set; } 
        public override DateTime FinishDate { get; set; }
        public override State State { get; set; }
        public IEnumerable<SubTask> SubTasks { get; set; } = new List<SubTask>();
        public int ProjectId { get; set; }
    }
}
