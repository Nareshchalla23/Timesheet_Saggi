using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Timesheet_demo2.Models
{
    public class TimesheetViewModel
    {
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        public double HoursWorked { get; set; }
        public string? Description { get; set; }

        // For the dropdown list
        public List<SelectListItem> Projects { get; set; } = new List<SelectListItem>();
    }
}
