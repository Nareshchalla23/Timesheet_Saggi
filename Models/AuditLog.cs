using System;
using System.Collections.Generic;

namespace TImesheet_demo2.Models
{
    public partial class AuditLog
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string ActionType { get; set; } = null!;
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
