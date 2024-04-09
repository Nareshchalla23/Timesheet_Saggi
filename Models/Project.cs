using System;
using System.Collections.Generic;

namespace TImesheet_demo2.Models
{
    public partial class Project
    {
        public Project()
        {
            Timesheets = new HashSet<Timesheet>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public string ProjectCode { get; set; } = null!;
        public int? ProjectManagerId { get; set; }

        public virtual User? ProjectManager { get; set; }
        public virtual ICollection<Timesheet> Timesheets { get; set; }
    }
}
