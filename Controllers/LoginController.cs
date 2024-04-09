using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TImesheet_demo2.Models;
using YourNamespace.Utilities;

namespace YourNamespace.Controllers
{
    public class LoginController : Controller
    {
        private readonly Timesheet_Demo2Context _context;

        public LoginController(Timesheet_Demo2Context context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.EmployeeId == model.EmployeeID);

                if (user != null && HashPassword.Verify(model.Password, user.PasswordHash))
                {
                    HttpContext.Session.SetString("UserID", user.UserId.ToString());
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetInt32("RoleID", (int)user.RoleId);
                    if (user.RoleId == 2)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Create", "Timesheet");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }
    }
}
