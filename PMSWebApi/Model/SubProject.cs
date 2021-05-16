using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSWebApi.Model
{
    public class SubProjectModel : ProjectBase
    {

        public override int Id { get; set; }
        [Required]
        public override string Code { get; set; }
        public override string Name { get; set; }
        public override DateTime StartDate { get; set; }
        public override DateTime FinishDate { get; set; }
        public override State State { get; set; }
        public int ProjectId { get; set; }
        public IEnumerable<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
