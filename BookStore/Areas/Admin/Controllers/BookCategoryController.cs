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
    public class BookCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IBookRepository _repository;

        public BookCategoryController(ApplicationDbContext db, IBookRepository repository)
        {
            _db = db;
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View(_repository.GetAllCategories());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookCategory category)
        {
            if(ModelState.IsValid)
            {
                _repository.AddCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            var category = _repository.GetCategoryById(id);
            if(category == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookCategory category)
        {
            if(ModelState.IsValid)
            {
                _repository.UpdateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            var category = _repository.GetCategoryById(id);
            if (category == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            var category = _repository.GetCategoryById(id);
            if (category == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(BookCategory category)
        {
            _repository.DeleteCategory(category);
            return RedirectToAction(nameof(Index));
        }
    }
}