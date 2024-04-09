using System;
using System.Collections.Generic;

namespace TImesheet_demo2.Models.ViewModels
{
    public class TimesheetFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
    }
}
