using System;
using System.Collections.Generic;

namespace TImesheet_demo2.Models
{
    public partial class Timesheet
    {
        public int TimesheetId { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public double HoursWorked { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = "pending";
        public string? ManagerComments { get; set; }

        // Navigation properties
        public virtual Project? Project { get; set; }
        public virtual User? User { get; set; }
    }

}
