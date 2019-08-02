using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IBookRepository _repository;

        public AuthorController(ApplicationDbContext db, IBookRepository repository)
        {
            _db = db;
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View(_repository.GetAllAuthors());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _repository.AddAuthor(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            var author = _repository.GetAuthorById(id);
            if (author == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            return View(author);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateAuthor(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            var author = _repository.GetAuthorById(id);
            if (author == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            return View(author);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            var author = _repository.GetAuthorById(id);
            if (author == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            return View(author);
        }
        [HttpPost]
        public IActionResult Delete(Author author)
        {
            _repository.DeleteAuthor(author);
            return RedirectToAction(nameof(Index));
        }
    }
}