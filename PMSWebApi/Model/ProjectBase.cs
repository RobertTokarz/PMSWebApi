using System;
using System.Collections.Generic;
using System.Text;

namespace PMSWebApi.Model
{
    public abstract class ProjectBase
    {
        public abstract int Id { get; set; }
        public abstract string Code { get; set; }
        public abstract string Name { get; set; }
        public abstract DateTime StartDate { get; set; }
        public abstract DateTime FinishDate { get; set; }
        public abstract State State { get; set; }

    }
}
