using System;
using System.Collections.Generic;

namespace TImesheet_demo2.Models
{
    public partial class User
    {
        public User()
        {
            AuditLogs = new HashSet<AuditLog>();
            Projects = new HashSet<Project>();
            Reports = new HashSet<Report>();
            Timesheets = new HashSet<Timesheet>();
        }

        public int UserId { get; set; }
        public string EmployeeId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<AuditLog> AuditLogs { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Timesheet> Timesheets { get; set; }
    }
}
