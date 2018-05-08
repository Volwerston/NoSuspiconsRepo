using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Film.Data;
using Film.Models;
using FilmDatabase.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FilmDatabase.Controllers
{
    [Culture]
    public class NavigationController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NavigationController(ApplicationDbContext context)
        {
            _context = context;
        }
        //
        // GET: /Navigation/
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            List<string> categories = _context.Categories.Select(x => x.Name).Distinct().OrderBy(x => x).ToList();
            return PartialView(categories);
        }

        public PartialViewResult Search(string search)
        {
            ViewBag.Search = search;
            return PartialView();
        }
        public ActionResult ShowGenres(bool pressed)
        {
            if (!pressed)
            {
                TempData["Pressed"] = "true";
            }
            else
            {
                TempData["Pressed"] = "false";
            }
            TempData.Keep("Pressed");
            return RedirectToAction("Index", "Home");
        }
        // public ActionResult ChangeCulture(string lang)
        // {
        //     string returnUrl = Request.UrlReferrer.AbsolutePath;
        //     // Список культур
        //     List<string> cultures = new List<string>() { "ru", "en", "de" };
        //     if (!cultures.Contains(lang))
        //     {
        //         lang = "ru";
        //     }
        //     // Сохраняем выбранную культуру в куки
        //     var cookie = Request.Cookies["lang"];
        //     if (cookie != null)
        //         cookie = lang;   // если куки уже установлено, то обновляем значение
        //     else
        //     {

        //         cookie = new HttpCookie("lang");
        //         cookie.HttpOnly = false;
        //         cookie.Value = lang;
        //         cookie.Expires = DateTime.Now.AddYears(1);
        //     }
        //     Response.Cookies.Add(cookie);
        //     return Redirect(returnUrl);
        // }

    }

    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public MenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(string category)
        {
            ViewBag.SelectedCategory = category;
            List<string> categories = _context.Categories.Select(x => x.Name).Distinct().OrderBy(x => x).ToList();
            return View(categories);
        }
    }
    
    public class SearchViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string search)
        {
            ViewBag.Search = search;
            return View();
        }
    }
}