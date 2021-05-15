using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSWebApi.Model
{
    public class Project : ProjectBase
    {      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get ; set; }
        [Required]
        public override string Code { get ; set; }
        public override string Name { get; set; }
        public override DateTime StartDate { get; set; }
        public override DateTime FinishDate { get; set; }
        public override State State { get; set ; }
        public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
        public IEnumerable<SubProject> SubProjects { get; set; } = new List<SubProject>();
    }
}
