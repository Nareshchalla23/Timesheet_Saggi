using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TImesheet_demo2.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; 

namespace Timesheet_demo2.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly Timesheet_Demo2Context _context;
        private readonly ILogger<TimesheetController> _logger; 

        public TimesheetController(Timesheet_Demo2Context context, ILogger<TimesheetController> logger)
        {
            _context = context;
            _logger = logger; 
        }

        // GET: Timesheet/Create
        public async Task<IActionResult> Create()
        {
            try
            {
               
                ViewData["ProjectId"] = new SelectList(await _context.Projects.ToListAsync(), "ProjectId", "ProjectName");

        
                var userIdString = HttpContext.Session.GetString("UserID");
                if (!int.TryParse(userIdString, out var userId))
                {
                    _logger.LogError("UserID not found in session.");
                    return RedirectToAction("Index", "Login");
                }
                var user = await _context.Users.FindAsync(userId);
                ViewData["UserName"] = user?.Name ?? "Unknown User";

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading projects for timesheet creation.");
                ModelState.AddModelError(string.Empty, "An error occurred while loading projects. Please try again later.");
                return View();
            }
        }

        // POST: Timesheet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Date,StartTime,EndTime,HoursWorked,Description")] Timesheet timesheet)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdString = HttpContext.Session.GetString("UserID");
                    if (!int.TryParse(userIdString, out var userId))
                    {
                        _logger.LogError("UserID not found in session.");
                        ModelState.AddModelError(string.Empty, "An error occurred. Please log in again.");
                        return View();
                    }

                  
                    timesheet.UserId = userId;
                    timesheet.Status = "Pending";

                    _context.Add(timesheet);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Timesheet entry created successfully.");
                    return RedirectToAction(nameof(Index)); 
                }

            
                ViewData["ProjectId"] = new SelectList(await _context.Projects.ToListAsync(), "ProjectId", "ProjectName", timesheet.ProjectId);
                return View(timesheet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating timesheet entry.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the timesheet entry. Please try again later.");
                return View(timesheet);
            }
        }

    
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var userIdString = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdString, out var userId))
            {
                _logger.LogError("UserID not found in session.");
                return RedirectToAction("Index", "Login");
            }

            var timesheets = _context.Timesheets
                .Where(t => t.UserId == userId && (!startDate.HasValue || t.Date >= startDate.Value) && (!endDate.HasValue || t.Date <= endDate.Value))
                .OrderByDescending(t => t.Date);

         
            var user = await _context.Users.FindAsync(userId);
            ViewData["UserName"] = user?.Name ?? "Unknown User";

            return View(await timesheets.ToListAsync());
        }

    
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timesheet = await _context.Timesheets
                .FirstOrDefaultAsync(m => m.TimesheetId == id);
            if (timesheet == null)
            {
                return NotFound();
            }

            return View(timesheet);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);
            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

   
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet == null)
            {
                return NotFound();
            }

            ViewData["ProjectId"] = new SelectList(await _context.Projects.ToListAsync(), "ProjectId", "ProjectName", timesheet.ProjectId);

            return View(timesheet);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimesheetId,ProjectId,Date,StartTime,EndTime,HoursWorked,Description")] Timesheet timesheet)
        {
            if (id != timesheet.TimesheetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timesheet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimesheetExists(timesheet.TimesheetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

        
            ViewData["ProjectId"] = new SelectList(await _context.Projects.ToListAsync(), "ProjectId", "ProjectName", timesheet.ProjectId);

            return View(timesheet);
        }

        private bool TimesheetExists(int id)
        {
            return _context.Timesheets.Any(e => e.TimesheetId == id);
        }

    }
}
