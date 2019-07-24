using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AuthorController(ApplicationDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            var Authors = _db.Authors.Include(a => a.Books).ThenInclude(b => b.Category);
            return View(Authors);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Author = await _db.Authors.Include(a => a.Books).ThenInclude(b => b.Category)
                        .SingleOrDefaultAsync(a => a.AuthorId == id);
            if (Author == null)
            {
                return NotFound();
            }
            return View(Author);
        }
    }
}