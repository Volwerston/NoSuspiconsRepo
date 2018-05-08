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
using Film.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Film.Controllers
{
    [Culture]
    public class ModeratorController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ModeratorController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Authorize(Roles = "moderator")]
        public ViewResult Index()
        {
            var films = _context.Films.Include(c => c.Categories)
            .ThenInclude(c => c.Category)
            .Include(c => c.Marks)
            .Include(c => c.Comments).ToList();

            ViewBag.Count = films.Count;
            return View(films);
        }
        [Authorize(Roles = "moderator")]
        [HttpGet]
        public ActionResult DeleteComment(int id)
        {
            var c = _context.Comments.Find(id);
            if (c == null)
            {
                return NotFound();
            }
            return View(c);
        }
        [Authorize(Roles = "moderator")]
        [HttpPost, ActionName("DeleteComment")]
        public ActionResult DeleteCommentConfirmed(int id)
        {
            var c = _context.Comments.Find(id);

            if (c == null)
            {
                return NotFound();
            }
            int filmId = c.FilmId;
            _context.Comments.Remove(c);
            _context.SaveChanges();
            return RedirectToAction("Details", "Home", new { id = filmId });

        }
        [Authorize(Roles = "moderator")]
        public ActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            var model = new FilmViewModel();
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "moderator")]
        //  [ValidateAntiForgeryToken]
        public ActionResult Create(FilmViewModel model)
        {
            var imageTypes = new string[]{    
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
                };
            //  if (model.Image == null || model.Image.ContentLength == 0)
            //   {
            //      ModelState.AddModelError("ImageUpload", "Додайте зображення");
            //   }
            if (model.Image != null && !imageTypes.Contains(model.Image.ContentType))
            {
                ModelState.AddModelError("Image", "Зображення повинне бути у GIF, JPG або PNG форматі.");
            }
            else if (model.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "Додайте жанр");
            }

            if (ModelState.IsValid)
            {
                var film = new Movie();
                film.Name = model.Name;
                film.Description = model.Description;
                if (model.CategoryId != null)
                {
                    foreach (var c in model.CategoryId)
                    {
                        if (_context.Categories.Find(c) != null)
                        {
                            var categoryFilm = new CategoryFilm();
                            categoryFilm.Category = _context.Categories.Find(c);
                            categoryFilm.Film = film;
                            film.Categories.Add(categoryFilm);
                        }
                    }

                }
                if (model.Image != null)
                {
                    var bytes = BytesFromStream(model.Image.OpenReadStream());
                    ViewBag.Width = 100;
                    ViewBag.Height = 100;
                    film.Image = bytes;
                }
                _context.Films.Add(film);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(model);
        }
        [Authorize(Roles = "moderator")]
        public ActionResult Delete(int id)
        {
            var f = _context.Films.Find(id);
            if (f == null)
            {
                return NotFound();
            }
            return View(f);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "moderator")]
        public ActionResult DeleteConfirmed(int id)
        {
            var f = _context.Films.Find(id);
            if (f == null)
            {
                return NotFound();
            }
            _context.Films.Remove(f);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "moderator")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            var f = _context.Films.Find(id);
            return View(f);
        }
        [HttpPost]
        [Authorize(Roles = "moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie model, int[] selectedCategories, IFormFile upload = null)
        {
            if (ModelState.IsValid)
            {
                var film = _context.Films.Find(model.Id);
                if (film == null)
                {
                    return BadRequest();
                }
                film.Name = model.Name;
                film.Description = model.Description;
                if (selectedCategories != null)
                {
                    film.Categories.Clear();
                    foreach (var c in selectedCategories)
                    {
                        if (_context.Categories.Find(c) != null)
                        {
                            var categoryFilm = new CategoryFilm();
                            categoryFilm.Category = _context.Categories.Find(c);
                            categoryFilm.Film = film;
                            film.Categories.Add(categoryFilm);
                        }
                    }

                }
                if (upload != null)
                {
                    var bytes = BytesFromStream(upload.OpenReadStream());
                    ViewBag.Width = 100;
                    ViewBag.Height = 100;
                    film.Image = bytes;
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        private static byte[] BytesFromStream(Stream stream)
        {
            if(stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            else
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
	}
}