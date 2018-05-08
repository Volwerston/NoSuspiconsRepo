using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Film.Models;
using System.IO;
using System.Drawing;
using System.Net;
using System.Text;
using FilmDatabase.Filters;
using Microsoft.AspNetCore.Mvc;
using Film.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FilmDatabase.Controllers
{
    [Culture]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
     
        [Authorize(Roles = "admin")]
        public async Task<ViewResult> ViewUsers()
        {
            var user = await _userManager.GetUserAsync(User);
            var users = _context.Users.Where(c => c.Id != user.Id);
            ViewBag.UserCount = (await _userManager.GetUsersInRoleAsync("user")).Count;
            return View(users.ToList());
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Ban(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user.Blocked)
            {
                user.Blocked = false;
            }
            else
            {
                user.Blocked = true;
            }

            _context.SaveChanges();
            return RedirectToAction("ViewUsers", "Admin");
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> PromoteToRole(string id, string role)
        {
          //  var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            
            var user = await _userManager.FindByIdAsync(id);
            var role1 = await _roleManager.FindByIdAsync(role);
            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction("ViewUsers", "Admin");
        }

    }
}