using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IBookRepository _repository;

        public CategoryController(ApplicationDbContext db, IBookRepository repository)
        {
            this._db = db;
            _repository = repository;
        }
        public IActionResult Index()
        {
            var Categories = _repository.GetAllCategoriesWithDetails();
            return View(Categories);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            var Category = _repository.GetCategoryDetailsById(id);
            if (Category == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            return View(Category);
        }
    }
}