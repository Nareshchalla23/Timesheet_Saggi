using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TImesheet_demo2.Models;
using X.PagedList; // Ensure this namespace is included

namespace TImesheet_demo2.Controllers
{
    public class DashboardController : Controller
    {
        private readonly Timesheet_Demo2Context _context;

        public DashboardController(Timesheet_Demo2Context context)
        {
            _context = context;
        }

        // GET: Dashboard
        public IActionResult Index(int? userId, DateTime? startDate, DateTime? endDate, int? page)
        {
            var timesheets = _context.Timesheets.AsQueryable();

            if (userId.HasValue)
            {
                timesheets = timesheets.Where(t => t.UserId == userId.Value);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                timesheets = timesheets.Where(t => t.Date >= startDate.Value && t.Date <= endDate.Value);
            }

            int pageSize = 10; // Set the number of items per page
            int pageNumber = (page ?? 1); // Set the current page number

            // Convert the queryable to a paginated list using X.PagedList
            var paginatedTimesheets = timesheets.ToPagedList(pageNumber, pageSize);

            return View(paginatedTimesheets);
        }
// POST: Dashboard/UpdateStatus
[HttpPost]
        public IActionResult UpdateStatus(int timesheetId, string status)
        {
            var timesheet = _context.Timesheets.Find(timesheetId);
            if (timesheet != null)
            {
                timesheet.Status = status;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Dashboard/ApproveAll
        [HttpPost]
        public IActionResult ApproveAll(int userId)
        {
            var timesheets = _context.Timesheets.Where(t => t.UserId == userId);
            foreach (var timesheet in timesheets)
            {
                timesheet.Status = "Approved";
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
