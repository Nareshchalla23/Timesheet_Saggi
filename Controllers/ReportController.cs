using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TImesheet_demo2.Models;
using TImesheet_demo2.Models.ViewModels;

namespace TImesheet_demo2.Controllers
{
    public class ReportController : Controller
    {
        private readonly Timesheet_Demo2Context _context;

        public ReportController(Timesheet_Demo2Context context)
        {
            _context = context;
        }

        // GET: Report
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            if (!HttpContext.Session.TryGetValue("UserID", out var userIdValue))
            {
                return Unauthorized("User is not logged in.");
            }

            int userId = int.Parse(System.Text.Encoding.UTF8.GetString(userIdValue));
            var viewModel = new TimesheetFilterViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Timesheets = await _context.Timesheets
                    .Where(t => t.UserId == userId && (!startDate.HasValue || t.Date >= startDate.Value) && (!endDate.HasValue || t.Date <= endDate.Value))
                    .Include(t => t.Project)
                    .Include(t => t.User)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Report/Download
        public async Task<IActionResult> Download(DateTime? startDate, DateTime? endDate)
        {
            if (!HttpContext.Session.TryGetValue("UserID", out var userIdValue))
            {
                return Unauthorized("User is not logged in.");
            }

            int userId = int.Parse(System.Text.Encoding.UTF8.GetString(userIdValue));
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Timesheet");
                worksheet.Cell("A1").Value = "Week Number";
                worksheet.Cell("B1").Value = "Project";
                worksheet.Cell("C1").Value = "Employee ID";
                worksheet.Cell("D1").Value = "Date";
                worksheet.Cell("E1").Value = "Start Time";
                worksheet.Cell("F1").Value = "End Time";
                worksheet.Cell("G1").Value = "Hours Worked";
                worksheet.Cell("H1").Value = "Status";
                worksheet.Cell("I1").Value = "Total Hours This Week";

                var timesheets = await _context.Timesheets
                    .Where(t => t.UserId == userId && (!startDate.HasValue || t.Date >= startDate.Value) && (!endDate.HasValue || t.Date <= endDate.Value))
                    .Include(t => t.Project)
                    .Include(t => t.User)
                    .OrderBy(t => t.Date)
                    .ToListAsync();

                int currentRow = 2;
                int currentWeek = 0;
                double totalHoursWeek = 0;

                foreach (var timesheet in timesheets)
                {
                    int weekOfYear = ISOWeek.GetWeekOfYear(timesheet.Date);
                    if (weekOfYear != currentWeek)
                    {
                        if (currentWeek != 0)
                        {
                            worksheet.Cell(currentRow - 1, 9).Value = totalHoursWeek;
                            totalHoursWeek = 0;
                        }
                        currentWeek = weekOfYear;
                    }

                    worksheet.Cell(currentRow, 1).Value = weekOfYear;
                    worksheet.Cell(currentRow, 2).Value = timesheet.Project?.ProjectName;
                    worksheet.Cell(currentRow, 3).Value = timesheet.UserId;
                    worksheet.Cell(currentRow, 4).Value = timesheet.Date.ToString("yyyy-MM-dd");
                    worksheet.Cell(currentRow, 5).Value = timesheet.StartTime.ToString(@"hh\:mm");
                    worksheet.Cell(currentRow, 6).Value = timesheet.EndTime.ToString(@"hh\:mm");
                    worksheet.Cell(currentRow, 7).Value = timesheet.HoursWorked;
                    worksheet.Cell(currentRow, 8).Value = timesheet.Status;

                    // Apply color schema based on the status
                    var cell = worksheet.Cell(currentRow, 8);
                    switch (timesheet.Status.ToLower())
                    {
                        case "approved":
                            cell.Style.Fill.BackgroundColor = XLColor.Green;
                            break;
                        case "pending":
                            cell.Style.Fill.BackgroundColor = XLColor.Yellow;
                            break;
                        case "rejected":
                            cell.Style.Fill.BackgroundColor = XLColor.Red;
                            break;
                    }

                    totalHoursWeek += timesheet.HoursWorked;
                    currentRow++;
                }

                // Set total hours for the last week
                if (currentWeek != 0)
                {
                    worksheet.Cell(currentRow - 1, 9).Value = totalHoursWeek;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TimesheetReport.xlsx");
                }
            }
        }
    }
}
