using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            var Categories = _db.BookCategories.Include(c => c.Books).ThenInclude(b => b.Author);
            return View(Categories);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Category = await _db.BookCategories.Include(c => c.Books).ThenInclude(b => b.Author)
                            .SingleOrDefaultAsync(c => c.Id == id);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }
    }
}