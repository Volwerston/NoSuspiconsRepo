using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Film.Models;
using FilmDatabase.Filters;
using Film.Data;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Film.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Search([FromForm]SearchModel searchModel)
        {
            return RedirectToAction("Index", new {search = searchModel.Search});
        }

        [HttpGet]
        public IActionResult Index(string search = "", bool rated = false, string[] selectedCategories = null)
        {
            var query = _applicationDbContext
            .Films
            .Include(c => c.Marks)
            .Include(c => c.Comments)
            .Include(c => c.Categories)
            .ThenInclude(x => x.Category)
            .AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.ToLowerInvariant() == search.ToLowerInvariant());
            }

            if(selectedCategories != null)
            {
                query = query.Where(film => selectedCategories.All(cat => film.Categories.Any(fcat => fcat.Category.Name == cat)));
            }

            if(rated)
            {
                query = query.Where(c => c.Marks.Count != 0).OrderByDescending(c => c.Marks.Average(m => m.MarkValue));
            }

            var data = query.ToList();
            return View(data);
        }

        public ActionResult Filter(string[] selectedCategories)
        {
            return RedirectToAction("Index", new {selectedCategories = selectedCategories});
        }

        [HttpGet]
        public ActionResult CreateComment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateComment(Comment comment)
        {
            int filmId = Convert.ToInt32(TempData["FilmId"]);
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                comment.UserName = User.Identity.Name;
              
                Movie film = _applicationDbContext.Films.Find(filmId);
                comment.Film = film;

                _applicationDbContext.Comments.Add(comment);
                _applicationDbContext.SaveChanges();
            }

           return RedirectToAction("Details", new { id = filmId });
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Movie product = _applicationDbContext.Films
            .Include(c => c.Comments)
            .Include(c => c.Marks)
            .Include(c => c.Categories)
            .ThenInclude(c => c.Category)
            .FirstOrDefault(c => c.Id == id);
            
            if (product == null)
            {
                return NotFound();
            }
           
            //ApplicationDbContext db = new ApplicationDbContext();
            
         //   ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var aList = list.Select((x, i) => new { Value = x, Data = x }).ToList();
            ViewBag.List = new SelectList(aList, "Value", "Data");
            return View(product);
        }

        public async Task<ActionResult> PutMark(Mark mark)
        {
            int filmId = Convert.ToInt32(TempData["FilmId"]);
            if (filmId != 0)
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    
                    if (_applicationDbContext.Marks.Any(a => a.FilmId == filmId && a.UserId == user.Id))
                    {
                        Mark m = _applicationDbContext.Marks.Include(c => c.Film).FirstOrDefault(c => c.FilmId == filmId && c.UserId == user.Id);
                        m.MarkValue = mark.MarkValue;
                        m.UserName = user.Email;
                        _applicationDbContext.SaveChanges();
                    }
                    else
                    {
                        mark.UserId = user.Id;
                        Movie film = _applicationDbContext.Films.Find(filmId);
                        mark.Film = film;
                        mark.UserName = user.Email;
                        _applicationDbContext.Marks.Add(mark);
                        _applicationDbContext.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Details", new { id = filmId });  
        }
    }
}
