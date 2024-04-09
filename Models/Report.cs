using System;
using System.Collections.Generic;

namespace TImesheet_demo2.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public int GeneratedByUserId { get; set; }
        public string ReportType { get; set; } = null!;
        public DateTime DateGenerated { get; set; }
        public string? FiltersApplied { get; set; }

        public virtual User GeneratedByUser { get; set; } = null!;
    }
}
